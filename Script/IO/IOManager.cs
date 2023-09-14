using System.Collections.Generic;
using System.Text;

namespace ArmyAnt.IO {
    public partial class IOManager {
        public static readonly IOManager Instance = new() { Root = UnityEngine.Application.persistentDataPath };

        public static UnityEngine.Networking.UnityWebRequest HttpGet(string url) {
            var request = UnityEngine.Networking.UnityWebRequest.Get(url);
            return request;
        }

        public static UnityEngine.Networking.UnityWebRequest HttpPost(string url, string fieldData) {
            var request = UnityEngine.Networking.UnityWebRequest.PostWwwForm(url, fieldData);
            return request;
        }

        public static UnityEngine.Networking.UnityWebRequest HttpPost(string url, Dictionary<string, string> fields) {
            var request = UnityEngine.Networking.UnityWebRequest.Post(url, fields);
            return request;
        }

        public static UnityEngine.Networking.UnityWebRequest HttpPost(string url, Dictionary<string, string> fields, Dictionary<string, byte[]> binaries) {
            var form = new UnityEngine.WWWForm();
            if(fields != null) {
                foreach(var post_arg in fields) {
                    form.AddField(post_arg.Key, post_arg.Value);
                }
            }
            if(binaries != null) {
                foreach(var post_arg in binaries) {
                    form.AddBinaryData(post_arg.Key, post_arg.Value);
                }
            }
            var request = UnityEngine.Networking.UnityWebRequest.Post(url, fields);
            return request;
        }

        public static UnityEngine.Networking.UnityWebRequest HttpPost(string url, Dictionary<string, string> fields, Dictionary<string, (byte[] contents, string filename, string mimetype)> binaries) {
            var form = new UnityEngine.WWWForm();
            if(fields != null) {
                foreach(var post_arg in fields) {
                    form.AddField(post_arg.Key, post_arg.Value);
                }
            }
            if(binaries != null) {
                foreach(var post_arg in binaries) {
                    form.AddBinaryData(post_arg.Key, post_arg.Value.contents, post_arg.Value.filename, post_arg.Value.mimetype);
                }
            }
            var request = UnityEngine.Networking.UnityWebRequest.Post(url, fields);
            return request;
        }
    }
}
