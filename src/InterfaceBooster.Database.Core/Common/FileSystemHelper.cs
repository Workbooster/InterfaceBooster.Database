using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceBooster.Database.Core.Common
{
    public static class FileSystemHelper
    {
        /// <summary>
        /// check whether the given directory path is writable for the current application
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static bool IsDirectoryWritable(string directoryPath)
        {
            bool isWriteAccess = false;

            try
            {
                AuthorizationRuleCollection collection = Directory.GetAccessControl(directoryPath).GetAccessRules(true, true, typeof(NTAccount));

                foreach (FileSystemAccessRule rule in collection)
                {
                    if (rule.AccessControlType == AccessControlType.Allow)
                    {
                        isWriteAccess = true;
                        break;
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                isWriteAccess = false;
            }
            catch (Exception)
            {
                isWriteAccess = false;
            }

            return isWriteAccess;
        }
    }
}
