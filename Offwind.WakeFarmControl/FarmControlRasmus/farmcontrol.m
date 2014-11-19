%% The main file for running the wind farm controll and wake simulation.
clear all; clc;

%% Initialization
%General settings to be changed
saveData                = true; % Save all the simulated data to a .mat file?
enablePowerDistribution = true; % Enable wind farm control and not only constant power
enableTurbineDynamics   = true; % Enable dynamical turbine model. Disabling this will increase the speed significantly, but also lower the fidelity of the results (setting to false does not work properly yet)
powerRefInterpolation   = true; % Power Reference table interpolation.
enableVaryingDemand     = true; % Varying Reference

% Simulation Properties:
simParm.tStart      = 0; % time start
simParm.timeStep    = 0.1; % time step, 8Hz - the NREL model is 80Hz (for reasons unknown)
simParm.tEnd        = 100; % time end
simParm.gridRes     = 10; % Grid Resolution
simParm.grid        = 1000; % Grid Size
simParm.ctrlUpdate  = 5;  % Update inverval for farm controller
simParm.powerUpdate = 1; % How often the control algorithm should update!
load('wind.mat'); % Load Wind Data

% Wind farm and Turbine Properties properties
parm.wf     = load('initial_data'); % Loads the Wind Farm Layout.
parm.N      = length(parm.wf); % number of turbines in farm
parm.rotA   = -48.80; % Angle of Attack
parm.kWake  = 0.06;

%% Turbine properties - Loaded from the NREL5MW.mat file
load NREL5MW.mat % Load parameters from the NREL 5MW Reference turbine struct.
parm.rho        = env.rho;% air density
parm.radius     = wt.rotor.radius*ones(1,parm.N); % rotor radius (NREL5MW)
parm.rated      = wt.ctrl.p_rated*ones(1,parm.N); %rated power (NREL5MW)
parm.ratedSpeed = wt.rotor.ratedspeed; %rated rotor speed
[~, idx]        = max(wt.cp.table(:)); % Find index for max Cp
parm.Ct         = 0.0*wt.ct.table(idx)*ones(parm.N,((simParm.tEnd-simParm.tStart)/simParm.timeStep)); % Define initial Ct as the optimal Ct. 
parm.Cp         = wt.cp.table(idx)*ones(parm.N,((simParm.tEnd-simParm.tStart)/simParm.timeStep)); % Define initial Cp as the optimal Cp. 
Mg_max_rate     = wt.ctrl.torq.ratelim; % Rate-limit on Torque Change.

%Pitch control
Ki      = wt.ctrl.pitch.Igain; % 0.008068634*360/2/pi; % integral gain (NREL5MW).
Kp      = wt.ctrl.pitch.Pgain; % 0.01882681*360/2/pi; % proportional gain (NREL5MW).
Umax    = wt.ctrl.pitch.ulim; % Upper limit of the pitch controller
Umin    = wt.ctrl.pitch.llim; % Lower limit of the pitch controller.

% NREL Regional Control - extracted from the NREL report. 
VS_CtInSp     =      70.162240;
VS_RtGnSp     =     121.680500;
VS_Rgn2K      =       2.332287;

%% Set initial conditions
omega0  = wt.rotor.ratedspeed; % Desired Rotation speed
beta0   = 0; % wt.ctrl.pitch.llim; % Initial pitch at zero.
power0  = 0; % Power Production

%% Memory Allocation and Memory Initialization
initMatrix  = zeros(parm.N,((simParm.tEnd-simParm.tStart)/simParm.timeStep));
sumPower    = initMatrix(1,:); % Initialize produced power vector
sumRef      = initMatrix(1,:); % Initialize reference power vector
sumAvai     = initMatrix(1,:); % Initialize available power vector
P_ref_new   = initMatrix(1,:); % Initialize new reference vector
P_demand    = initMatrix(1,:); % Initialize power demand vector
v_nac       = initMatrix; % Initialize hub velocity matrix.
P_ref       = initMatrix; % Initialize matrix to save the power production history for each turbine.
Pa          = initMatrix; % Initialize available power matrix.
Power       = initMatrix; % Initialize individual WT power production matrix.
beta        = initMatrix; % Initialize pitch matrix.
Omega       = initMatrix; % Initialize revolutional velocity matrix.

initVector  = ones(parm.N,1);
Mg          = 0*initVector;
wField      = zeros(simParm.grid/simParm.gridRes,simParm.grid/simParm.gridRes); % Wind field matrix

%% Controller Initizliation
dZ          = 0; % Integrator Initialization.
du          = zeros(parm.N,1); % Integration variable. 
x           = [omega0*initVector 0*initVector]; % x0
u           = [beta0*initVector power0*initVector]; % u0
P_demand(1) = 50*5e6; % Power Demand.

%% Simulate wind farm operation
for i=2:((simParm.tEnd-simParm.tStart)/simParm.timeStep) % At each sample time (DT) from Tstart to Tend
    clc;
    fprintf('Iteration Counter: %i out of %i \n',i,simParm.tEnd*(1/simParm.timeStep));
    
    %%%%%%%%%%%%%%%% WIND FIELD FIFO MATRIX %%%%%%%%%%%%%%%%%%%
    wField(:,2:end) = wField(:,1:(end-1));
    wField(:,1)     = wind(i,2)*ones(simParm.grid/simParm.gridRes,1) + randn(simParm.grid/simParm.gridRes,1)*0.5;
    %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%  
    
    % Calculate the wake using the last Ct values
    v_nac(:,i)  = wakeCalculationsRLC(parm.Ct(:,i-1), transpose(wField), x(:,2), parm, simParm);
    x(:,2)      = v_nac(:,i);
    
    
    if (enableVaryingDemand) % A random walk to simulate fluctuations in the power demand.
        P_demand(i) = P_demand(i-1) + randn*50000;
    else
        P_demand(i) = P_demand(i-1);
    end
    
    % Farm control
    % Calculate the power distribution references for each turbine
    if (enablePowerDistribution)
        [P_ref_new, Pa(:,i)] = powerDistributionControl(x(:,2),P_demand(i),parm);
    end
    
    %Hold  the demand for some seconds
    if(mod(i,round(simParm.ctrlUpdate/simParm.timeStep)) == simParm.powerUpdate)
        P_ref(:,i) = P_ref_new;
    else
        if(powerRefInterpolation)
            alpha = 0.01;
            P_ref(:,i) = (1-alpha)*P_ref(:,i-1)+ (alpha)*P_ref_new;
        else
            P_ref(:,i) = P_ref_new;
        end
    end
    %Torque controller
    for j= 1:parm.N
        if ((x(j,1)*97 >= VS_RtGnSp) || (u(j,1) >= 1))      %! We are in region 3 - power is constant
            u(j,2)  = P_ref(j,i)./x(j,1);
        elseif (x(j,1)*97 <= VS_CtInSp)                     %! We are in region 1 - torque is zero
            u(j,2)  = 0.0;
        else                                                %! We are in region 2 - optimal torque is proportional to the square of the generator speed
            u(j,2)  = 97*VS_Rgn2K*x(j,1)*x(j,1)*97^2;
        end
    end
    
    dx      = (omega0 - x(:,1)) - (omega0 - Omega(:,i-1));
    du      = Kp*dx + Ki*simParm.timeStep*(omega0 - x(:,1));
    du      = min(max(du,-wt.ctrl.pitch.ratelim),wt.ctrl.pitch.ratelim);
    u(:,1)  = min(max(u(:,1)+du*simParm.timeStep,Umin),Umax);
 
    
    Mg(:,i)     = u(:,2); % Torque Input
    beta(:,i)   = u(:,1); % Pitch Input

    % Turbine dynamics - can be simplified:
    if(enableTurbineDynamics)    
        for j=1:parm.N
            [x(j,1), parm.Ct(j,i), parm.Cp(j,i)] = turbineDrivetrainModel(x(j,:) ,u(j,:) ,wt ,env ,simParm.timeStep);
        end
    else
        x(:,1)  = parm.ratedSpeed; % Rotational speed
    end
    
    Omega(:,i)  = x(:,1);
    Power(:,i)  = Omega(:,i).*Mg(:,i);
    
    % Power Summations
    sumPower(i) = sum(Power(:,i))*10^(-6);
    sumRef(i)   = sum(P_ref(:,i))*10^(-6);
    sumAvai(i)  = sum(Pa(:,i))*10^(-6);
    
    % NOWCASTING FUNKTION HER
    % powerPrediction(i) = powerPrediction(i,sumPower(i:-1:i-10)) % or something
    % similar.
end

%%
time    = (simParm.tStart:simParm.timeStep:simParm.tEnd-simParm.timeStep)';

if (saveData)
    dataOut = [time, sumPower', sumRef', sumAvai'];
    save dataOut;
end
%% Plotting
%Below a number of different plots are made. Most of them for test purposes
f1 = figure(1); clf;
plot(time,P_ref*(10^(-6))); grid on; % Power Reference
xlabel('Time [s]'); ylabel('Power Reference [MW]');
title('Individual Power Reference');
% 
f2 = figure(2); clf;
plot(time,v_nac); grid on; % Wind velocity at each turbine
xlabel('Time [s]'); ylabel('Wind Speed [m/s]');
title('Wind Speed @ individual turbine');

f3 = figure(3); clf;
plot(time,sumRef,time,sumAvai,time,sumPower); grid on; % Power References
xlabel('Time [s]'); ylabel('Power [MW]');
legend('Reference','Available','Produced');
title('Power Plot');

f4 = figure(4); clf;
plot(time,beta'); grid on;
xlabel('Time [s]'); ylabel('Pitch Angle [deg]');
title('Evolution of Pitch angle over time');

f5 = figure(5); clf;
plot(time,Omega'); grid on;
xlabel('Time [s]'); ylabel('Revolutional Velocity [rpm]');
title('Evolution of Revolutional Velocity over time');