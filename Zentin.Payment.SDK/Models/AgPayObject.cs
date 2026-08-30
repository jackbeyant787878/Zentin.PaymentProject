namespace Zentin.Payment.SDK.Models
{
    public class AgPayObject
    {
        /// <summary>
        /// other extend information
        /// </summary>
        protected Dictionary<string, object> extendInfos = new Dictionary<string, object>();

        /// <summary>
        /// get extend information
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetExtendInfos()
        {
            return extendInfos;
        }

        /// <summary>
        /// add extend information
        /// extened parameters have higher levels than entity parameters, and will overwrite existing keys, pay attention to capitalization
        /// </summary>
        /// <param name="extendInfos"></param>
        public void SetExtendInfo(Dictionary<string, object> extendInfos)
        {
            foreach (var ext in extendInfos)
            {
                this.extendInfos.Add(ext.Key, ext.Value);
            }
        }

        /// <summary>
        ///add extend information
        /// extended parameters have higher levels than entity parameters, and will overwrite existing keys, pay attention to capitalization
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddExtendInfo(string key, object value)
        {
            this.extendInfos.Add(key, value);
        }
    }
}
