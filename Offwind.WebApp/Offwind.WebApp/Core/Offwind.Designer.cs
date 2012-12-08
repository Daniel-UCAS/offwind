﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]

namespace Offwind.WebApp.Core
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class OffwindEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new OffwindEntities object using the connection string found in the 'OffwindEntities' section of the application configuration file.
        /// </summary>
        public OffwindEntities() : base("name=OffwindEntities", "OffwindEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new OffwindEntities object.
        /// </summary>
        public OffwindEntities(string connectionString) : base(connectionString, "OffwindEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new OffwindEntities object.
        /// </summary>
        public OffwindEntities(EntityConnection connection) : base(connection, "OffwindEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Page> Pages
        {
            get
            {
                if ((_Pages == null))
                {
                    _Pages = base.CreateObjectSet<Page>("Pages");
                }
                return _Pages;
            }
        }
        private ObjectSet<Page> _Pages;

        #endregion
        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Pages EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToPages(Page page)
        {
            base.AddObject("Pages", page);
        }

        #endregion
    }
    

    #endregion
    
    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="u254697_offwindModel", Name="Page")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Page : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Page object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="created">Initial value of the Created property.</param>
        /// <param name="updated">Initial value of the Updated property.</param>
        /// <param name="published">Initial value of the Published property.</param>
        /// <param name="homePage">Initial value of the HomePage property.</param>
        /// <param name="isPublished">Initial value of the IsPublished property.</param>
        /// <param name="votes">Initial value of the Votes property.</param>
        public static Page CreatePage(global::System.Int32 id, global::System.DateTime created, global::System.DateTime updated, global::System.DateTime published, global::System.Boolean homePage, global::System.Boolean isPublished, global::System.Int32 votes)
        {
            Page page = new Page();
            page.Id = id;
            page.Created = created;
            page.Updated = updated;
            page.Published = published;
            page.HomePage = homePage;
            page.IsPublished = isPublished;
            page.Votes = votes;
            return page;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Slug
        {
            get
            {
                return _Slug;
            }
            set
            {
                OnSlugChanging(value);
                ReportPropertyChanging("Slug");
                _Slug = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Slug");
                OnSlugChanged();
            }
        }
        private global::System.String _Slug;
        partial void OnSlugChanging(global::System.String value);
        partial void OnSlugChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String PageType
        {
            get
            {
                return _PageType;
            }
            set
            {
                OnPageTypeChanging(value);
                ReportPropertyChanging("PageType");
                _PageType = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("PageType");
                OnPageTypeChanged();
            }
        }
        private global::System.String _PageType;
        partial void OnPageTypeChanging(global::System.String value);
        partial void OnPageTypeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String PageTitle
        {
            get
            {
                return _PageTitle;
            }
            set
            {
                OnPageTitleChanging(value);
                ReportPropertyChanging("PageTitle");
                _PageTitle = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("PageTitle");
                OnPageTitleChanged();
            }
        }
        private global::System.String _PageTitle;
        partial void OnPageTitleChanging(global::System.String value);
        partial void OnPageTitleChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Title
        {
            get
            {
                return _Title;
            }
            set
            {
                OnTitleChanging(value);
                ReportPropertyChanging("Title");
                _Title = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Title");
                OnTitleChanged();
            }
        }
        private global::System.String _Title;
        partial void OnTitleChanging(global::System.String value);
        partial void OnTitleChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Announce
        {
            get
            {
                return _Announce;
            }
            set
            {
                OnAnnounceChanging(value);
                ReportPropertyChanging("Announce");
                _Announce = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Announce");
                OnAnnounceChanged();
            }
        }
        private global::System.String _Announce;
        partial void OnAnnounceChanging(global::System.String value);
        partial void OnAnnounceChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Text
        {
            get
            {
                return _Text;
            }
            set
            {
                OnTextChanging(value);
                ReportPropertyChanging("Text");
                _Text = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Text");
                OnTextChanged();
            }
        }
        private global::System.String _Text;
        partial void OnTextChanging(global::System.String value);
        partial void OnTextChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime Created
        {
            get
            {
                return _Created;
            }
            set
            {
                OnCreatedChanging(value);
                ReportPropertyChanging("Created");
                _Created = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Created");
                OnCreatedChanged();
            }
        }
        private global::System.DateTime _Created;
        partial void OnCreatedChanging(global::System.DateTime value);
        partial void OnCreatedChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime Updated
        {
            get
            {
                return _Updated;
            }
            set
            {
                OnUpdatedChanging(value);
                ReportPropertyChanging("Updated");
                _Updated = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Updated");
                OnUpdatedChanged();
            }
        }
        private global::System.DateTime _Updated;
        partial void OnUpdatedChanging(global::System.DateTime value);
        partial void OnUpdatedChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime Published
        {
            get
            {
                return _Published;
            }
            set
            {
                OnPublishedChanging(value);
                ReportPropertyChanging("Published");
                _Published = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Published");
                OnPublishedChanged();
            }
        }
        private global::System.DateTime _Published;
        partial void OnPublishedChanging(global::System.DateTime value);
        partial void OnPublishedChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Boolean HomePage
        {
            get
            {
                return _HomePage;
            }
            set
            {
                OnHomePageChanging(value);
                ReportPropertyChanging("HomePage");
                _HomePage = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("HomePage");
                OnHomePageChanged();
            }
        }
        private global::System.Boolean _HomePage;
        partial void OnHomePageChanging(global::System.Boolean value);
        partial void OnHomePageChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Url
        {
            get
            {
                return _Url;
            }
            set
            {
                OnUrlChanging(value);
                ReportPropertyChanging("Url");
                _Url = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Url");
                OnUrlChanged();
            }
        }
        private global::System.String _Url;
        partial void OnUrlChanging(global::System.String value);
        partial void OnUrlChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> Priority
        {
            get
            {
                return _Priority;
            }
            set
            {
                OnPriorityChanging(value);
                ReportPropertyChanging("Priority");
                _Priority = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Priority");
                OnPriorityChanged();
            }
        }
        private Nullable<global::System.Int32> _Priority;
        partial void OnPriorityChanging(Nullable<global::System.Int32> value);
        partial void OnPriorityChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Boolean IsPublished
        {
            get
            {
                return _IsPublished;
            }
            set
            {
                OnIsPublishedChanging(value);
                ReportPropertyChanging("IsPublished");
                _IsPublished = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("IsPublished");
                OnIsPublishedChanged();
            }
        }
        private global::System.Boolean _IsPublished;
        partial void OnIsPublishedChanging(global::System.Boolean value);
        partial void OnIsPublishedChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Votes
        {
            get
            {
                return _Votes;
            }
            set
            {
                OnVotesChanging(value);
                ReportPropertyChanging("Votes");
                _Votes = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Votes");
                OnVotesChanged();
            }
        }
        private global::System.Int32 _Votes;
        partial void OnVotesChanging(global::System.Int32 value);
        partial void OnVotesChanged();

        #endregion
    
    }

    #endregion
    
}