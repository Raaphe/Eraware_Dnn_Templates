﻿// MIT License
// Copyright $ext_companyname$

namespace $ext_rootnamespace$.Controllers
{
    using DotNetNuke.Security;
    using DotNetNuke.Web.Api;
    using NSwag.Annotations;
    using $ext_rootnamespace$.DTO;
    using $ext_rootnamespace$.Services;
    using $ext_rootnamespace$.ViewModels;
    using System;
    using System.Net;
    using System.Web.Http;

    /// <summary>
    /// Provides Web API access for items.
    /// </summary>
    public class ItemController : ModuleApiController
    {
        private readonly IItemService itemService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemController"/> class.
        /// </summary>
        /// <param name="itemService">The items reposioty.</param>
        public ItemController(IItemService itemService)
        {
            this.itemService = itemService;
        }

        /// <summary>
        /// Creates a new item.
        /// </summary>
        /// <param name="item">The item to create.</param>
        /// <returns>Nothing.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ItemViewModel), Description = "OK")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(string), Description = "Bad Request")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(Exception), Description = "Error")]
        public IHttpActionResult CreateItem(CreateItemDTO item)
        {
            try
            {
                var result = this.itemService.CreateItem(item, this.UserInfo.UserID);
                return this.Ok(result);
            }
            catch (ArgumentException ex)
            {
                this.Logger.Error(ex.Message, ex);
                return this.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                string message = "An unexpected error occured while trying to create the item";
                this.Logger.Error(message, ex);
                return this.InternalServerError(new Exception(message));
                throw;
            }
        }

    /// <summary>
    /// Gets a paged and sorted list of items matching a certain query.
    /// </summary>
    /// <param name="dto">The details of the query, <see cref="GetItemsPageDTO"/>.</param>
    /// <returns>List of pages + paging information.</returns>
    [HttpGet]
        [AllowAnonymous]
        [SwaggerResponse(
            HttpStatusCode.OK,
            typeof(ItemsPageViewModel),
            Description = "OK")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(Exception), Description = "Error")]
        public IHttpActionResult GetItemsPage([FromUri] GetItemsPageDTO dto)
        {
            try
            {
                return this.Ok(this.itemService.GetItemsPage(dto.Query, dto.Page, dto.PageSize, dto.Descending));
            }
            catch (Exception ex)
            {
                string message = "An unexpected error occured while trying to fetch items.";
                this.Logger.Error(message, ex);
                return this.InternalServerError(new Exception(message));
                throw;
            }
        }

        /// <summary>
        /// Deletes an existing item.
        /// </summary>
        /// <param name="itemId">The id of the item to delete.</param>
        /// <returns>Nothing.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "OK")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(Exception), Description = "Error")]
        public IHttpActionResult DeleteItem(int itemId)
        {
            try
            {
                this.itemService.DeleteItem(itemId);
                return this.Ok();
            }
            catch (Exception ex)
            {
                string message = "An unexpected error occured while trying to delete an item.";
                this.Logger.Error(message, ex);
                return this.InternalServerError(new Exception(message));
                throw;
            }
        }

        /// <summary>
        /// Checks if a user can edit the current items.
        /// </summary>
        /// <returns>A boolean indicating whether the user can edit the current items.</returns>
        [HttpGet]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, typeof(bool), Description = "OK")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(Exception), Description = "Error")]
        public IHttpActionResult UserCanEdit()
        {
            return this.Ok(this.CanEdit);
        }

        /// <summary>
        /// Updates an existing item.
        /// </summary>
        /// <param name="item">The new information about the item, <see cref="UpdateItemDTO"/>.</param>
        /// <returns>Only a status code and no data.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [SwaggerResponse(HttpStatusCode.OK, null, Description = "OK")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(ArgumentException), Description = "Malformed request")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(Exception), Description = "Error")]
        public IHttpActionResult UpdateItem(UpdateItemDTO item)
        {
            try
            {
                this.itemService.UpdateItem(item, this.UserInfo.UserID);
                return this.Ok();
            }
            catch (ArgumentException ex)
            {
                this.Logger.Error(ex.Message, ex);
                return this.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                var message = "An unexpected error occured trying to update the item.";
                this.Logger.Error(message, ex);
                return this.InternalServerError(new Exception(message));
                throw;
            }
        }
    }
}
