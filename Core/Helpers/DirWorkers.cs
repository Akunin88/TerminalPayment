namespace Core.Helpers
{
    public static class DirWorker
    {
        public static void RemoveDirectory(string directory, int attemtCount = 2)
        {
            for (int i = 0; i < attemtCount; i++)
            {
                try
                {
                    RemoveDirectory(directory, true);
                    return;
                }
                catch (System.Threading.ThreadAbortException) { throw; }
                catch
                {
                    Thread.Sleep(100);
                }
            }
            throw new IOException($"Could not create directory {directory}. To much attempts {attemtCount}");
        }

        public static void RemoveDirectory(string _Directory, bool Recursive = false)
        {
            if (_Directory == null || _Directory == "" || !Directory.Exists(_Directory))
                return;
            DirectoryInfo di = new DirectoryInfo(_Directory);
            try
            {
                SearchOption s_op = (Recursive) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                foreach (var fi in di.EnumerateFiles("*", s_op))
                {
                    if ((fi.Attributes & FileAttributes.ReadOnly) != 0)
                    {
                        fi.Attributes &= ~FileAttributes.ReadOnly;
                    }
                    File.Delete(fi.FullName);
                }
                Directory.Delete(di.FullName, Recursive);
            }
            catch (System.Threading.ThreadAbortException) { throw; }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Deleting directory [{0}]. {1}", _Directory, ex.Message));
            }
        }

        public static void CopyDirectory(string _oldDirectory, string _newDirectory)
        {
            if ((!Directory.Exists(_oldDirectory)) || ((Directory.Exists(_newDirectory))))
                return;
            DirectoryInfo di = new DirectoryInfo(_oldDirectory);
            try
            {
                foreach (var fi in di.EnumerateFiles("*", SearchOption.AllDirectories))
                {
                    string _subDir = (fi.Directory.Name == di.Name) ? fi.Name : fi.FullName.Substring(_oldDirectory.Length);
                    _subDir = Path.Combine(_subDir.Split('\\'));
                    _subDir = Path.Combine(_newDirectory, _subDir);
                    Path.GetDirectoryName(_subDir).CreateDirIfNotExists();
                    File.Copy(fi.FullName, Path.Combine(_subDir));
                }
                di = null;
            }
            catch (System.Threading.ThreadAbortException) { throw; }
            catch (Exception ex)
            {
                throw new Exception($"Copy directory [{_oldDirectory}] to [{_newDirectory}]. {ex.Message}");
            }
        }

        public static void CreateDirIfNotExists(this string DirName)
        {
            if (string.IsNullOrEmpty(DirName) || Directory.Exists(DirName))
                return;
            else
                Directory.CreateDirectory(DirName);
        }

        public static void DeleteFileIfExist(this string path, int attemptCount = 10)
        {
            if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path))
                return;
            for (int n = 0; n < attemptCount; n++)
            {
                try
                {
                    File.SetAttributes(path, FileAttributes.Archive);
                    System.IO.File.Delete(path);
                    return;
                }
                catch (System.Threading.ThreadAbortException) { throw; }
                catch
                {
                    Thread.Sleep(100);
                }
            }
            throw new IOException($"Could not delete file {path}. To much attempts ({attemptCount})");
        }

        public static void CopyFileIfExist(this string source, string destination, bool overwrite = true, int attemptCount = 1)
        {
            if (!File.Exists(source))
                return;
            for (int i = 0; i < attemptCount; i++)
            {
                try
                {
                    File.Copy(source, destination, overwrite);
                    return;
                }
                catch (Exception)
                {
                    Thread.Sleep(100);
                }
            }
            throw new IOException($"Could not copy file  {source}. To much attempts ({attemptCount})");
        }

        public static void DeleteDirectoryIfExist(this string path, bool recursive = false, int attemtsCount = 1)
        {
            for (int i = 0; i < attemtsCount; i++)
            {
                try
                {
                    DirWorker.RemoveDirectory(path, recursive);
                    return;
                }
                catch
                {
                    Thread.Sleep(300);
                }
            }
            throw new IOException($"Could not delete directory {path}. To much attempts {attemtsCount}");
        }

        public static void ClearDirectoryIfExist(this string path)
        {
            if (path == null || path == "")
                return;
            path.CreateDirIfNotExists();
            foreach (string dir in Directory.GetDirectories(path))
                dir.DeleteDirectoryIfExist(true, 10);
            foreach (string file in Directory.GetFiles(path))
                file.DeleteFileIfExist();
        }

        public static bool IsDirectoryEquals(string dir1, string dir2)
        {
            char[] sep = new char[] { '\\', '/', };
            var l1 = dir1.ToLower().Split(sep).ToList();
            var l2 = dir2.ToLower().Split(sep).ToList();
            for (int i = l1.Count - 1; i >= 0; i--)
            {
                if (l1[i].Trim() == "")
                    l1.RemoveAt(i);
            }
            for (int i = l2.Count - 1; i >= 0; i--)
            {
                if (l2[i].Trim() == "")
                    l2.RemoveAt(i);
            }
            if (l1.Count != l2.Count)
                return false;
            for (int i = 0; i < l1.Count; i++)
            {
                if (l1[i] != l2[i])
                    return false;
            }

            return true;
        }
    }
}
