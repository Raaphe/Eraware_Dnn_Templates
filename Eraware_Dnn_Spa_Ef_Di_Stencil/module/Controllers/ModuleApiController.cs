﻿// MIT License
// Copyright $ext_companyname$

namespace $ext_rootnamespace$.Controllers
{
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Instrumentation;
    using DotNetNuke.Security.Permissions;
    using DotNetNuke.Web.Api;
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Provides common features to all module controller.
    /// </summary>
    public abstract class ModuleApiController : DnnApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleApiController"/> class.
        /// </summary>
        public ModuleApiController()
        {
            this.Logger = LoggerSource.Instance.GetLogger(this.GetType());
        }

        /// <summary>
        /// Gets information about the Dnn user.
        /// </summary>
        public new virtual UserInfo UserInfo => base.UserInfo;

        /// <summary>
        /// Gets or sets a value indicating whether the user can edit this module.
        /// </summary>
        public virtual bool CanEdit
        {
            get
            {
                try
                {
                    return ModulePermissionController.HasModuleAccess(DotNetNuke.Security.SecurityAccessLevel.Edit, "EDIT", this.ActiveModule);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            [ExcludeFromCodeCoverage]
            set
            {
                throw new Exception("Only override this setter for testing.");
            }
        }

        /// <summary>
        /// Logs to the Dnn Log4Net logger.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:Property summary documentation should match accessors", Justification = "We are not really setting to the log, but logging.")]
        protected ILog Logger { get; }
    }
}
