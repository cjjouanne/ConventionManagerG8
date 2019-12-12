using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ConventionManager
{
    public interface IUpload
    {
        CloudBlobContainer GetPicturesContainer();
        CloudBlobContainer GetPdfsContainer();
    }
}