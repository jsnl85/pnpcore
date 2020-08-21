using Microsoft.Extensions.Logging;
using PnP.Core.Services;
using System.Net.Http;
using System.Threading.Tasks;

namespace PnP.Core.Model.SharePoint
{
    /// <summary>
    /// File class, write your custom code here
    /// </summary>
    [SharePointType("SP.File", Target = typeof(Folder), Uri = "_api/Web/getFileById('{Id}')", Get = "_api/Web/getFolderById('{Parent.Id}')/Files", LinqGet = "_api/Web/getFolderById('{Parent.Id}')/Files")]
    // TODO To implement when a token can be used to identify the parent list
    //[GraphType(Get = "sites/{hostname}:{serverrelativepath}/lists/{ParentList.Id}/items/{Id}")]
    internal partial class File
    {
        internal const string AddFileContentAdditionalInformationKey = "Content";
        internal const string AddFileOverwriteAdditionalInformationKey = "Overwrite";

        public File()
        {
            MappingHandler = (FromJson input) =>
            {
                // implement custom mapping logic
                switch (input.TargetType.Name)
                {
                    case nameof(CustomizedPageStatus): return JsonMappingHelper.ToEnum<CustomizedPageStatus>(input.JsonElement);
                    case nameof(PageRenderType): return JsonMappingHelper.ToEnum<ListPageRenderType>(input.JsonElement);
                }

                input.Log.LogDebug($"Field {input.FieldName} could not be mapped when converting from JSON");

                return null;
            };

            // TODO Continue implementation when no batch call will be supported
            //AddApiCallHandler = (additionalInformation) =>
            //{
            //    Stream content = (Stream)(additionalInformation.ContainsKey(AddFileContentAdditionalInformationKey)
            //        ? additionalInformation[AddFileContentAdditionalInformationKey]
            //        : null);
            //    if (content == null)
            //        throw new ClientException(ErrorType.InvalidParameters, "Adding new file without content is not possible, please provide valid content for the file");

            //    bool overwrite = (bool)(additionalInformation.ContainsKey(AddFileOverwriteAdditionalInformationKey)
            //        ? additionalInformation[AddFileOverwriteAdditionalInformationKey]
            //        : false);

            //};
        }


        #region Extensions
        #region Publish
        public async Task PublishAsync(string comment = null)
        {
            var entity = EntityManager.Instance.GetClassInfo(GetType(), this);
            string publishEndpointUrl = $"{entity.SharePointUri}/publish(comment='{comment ?? string.Empty}')";

            var apiCall = new ApiCall(publishEndpointUrl, ApiType.SPORest);

            await RawRequestAsync(apiCall, HttpMethod.Post).ConfigureAwait(false);
        }

        public void Publish(string comment = null)
        {
            PublishAsync(comment).GetAwaiter().GetResult();
        }

        public async Task PublishBatchAsync(string comment = null)
        {
            var entity = EntityManager.Instance.GetClassInfo(GetType(), this);
            string publishEndpointUrl = $"{entity.SharePointUri}/publish(comment='{comment ?? string.Empty}')";

            var apiCall = new ApiCall(publishEndpointUrl, ApiType.SPORest);

            await RawRequestBatchAsync(apiCall, HttpMethod.Post).ConfigureAwait(false);
        }

        public void PublishBatch(string comment = null)
        {
            PublishBatchAsync(comment).GetAwaiter().GetResult();
        }

        public async Task PublishBatchAsync(Batch batch, string comment = null)
        {
            var entity = EntityManager.Instance.GetClassInfo(GetType(), this);
            string publishEndpointUrl = $"{entity.SharePointUri}/publish(comment='{comment ?? string.Empty}')";

            var apiCall = new ApiCall(publishEndpointUrl, ApiType.SPORest);

            await RawRequestBatchAsync(batch, apiCall, HttpMethod.Post).ConfigureAwait(false);
        }

        public void PublishBatch(Batch batch, string comment = null)
        {
            PublishBatchAsync(batch, comment).GetAwaiter().GetResult();
        }
        #endregion

        #region Unpublish
        public void Unpublish(string comment = null)
        {
            UnpublishAsync(comment).GetAwaiter().GetResult();
        }

        public async Task UnpublishAsync(string comment = null)
        {
            var entity = EntityManager.Instance.GetClassInfo(GetType(), this);
            string publishEndpointUrl = $"{entity.SharePointUri}/unpublish(comment='{comment ?? string.Empty}')";

            var apiCall = new ApiCall(publishEndpointUrl, ApiType.SPORest);

            await RawRequestAsync(apiCall, HttpMethod.Post).ConfigureAwait(false);
        }

        public void UnpublishBatch(Batch batch, string comment = null)
        {
            UnpublishBatchAsync(batch, comment).GetAwaiter().GetResult();
        }

        public async Task UnpublishBatchAsync(string comment = null)
        {
            var entity = EntityManager.Instance.GetClassInfo(GetType(), this);
            string publishEndpointUrl = $"{entity.SharePointUri}/unpublish(comment='{comment ?? string.Empty}')";

            var apiCall = new ApiCall(publishEndpointUrl, ApiType.SPORest);

            await RawRequestBatchAsync(apiCall, HttpMethod.Post).ConfigureAwait(false);
        }

        public async Task UnpublishBatchAsync(Batch batch, string comment = null)
        {
            var entity = EntityManager.Instance.GetClassInfo(GetType(), this);
            string publishEndpointUrl = $"{entity.SharePointUri}/unpublish(comment='{comment ?? string.Empty}')";

            var apiCall = new ApiCall(publishEndpointUrl, ApiType.SPORest);

            await RawRequestBatchAsync(batch, apiCall, HttpMethod.Post).ConfigureAwait(false);
        }

        public void UnpublishBatch(string comment = null)
        {
            UnpublishBatchAsync(comment).GetAwaiter().GetResult();
        }
        #endregion
        #endregion
    }
}
