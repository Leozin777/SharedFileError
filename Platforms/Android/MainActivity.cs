using Android.App;
using Android.Content.PM;
using Android.Content;
using Android.OS;

namespace SharedFiles
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    [IntentFilter([Intent.ActionSend], Categories = [Intent.CategoryBrowsable, Intent.CategoryDefault, Intent.ActionView], DataMimeType = "image/*")]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Intent?.Action == Intent.ActionSend)
            {
                Stream? inputStream = null;
                var filePath = Intent?.ClipData?.GetItemAt(0);
                if (filePath?.Uri != null)
                {
                    inputStream = ContentResolver!.OpenInputStream(filePath.Uri)!;
                }

                if (inputStream != null)
                {
                    byte[] data = null;
                    using (var reader = new StreamReader(inputStream))
                    {
                        using (var memstream = new MemoryStream())
                        {
                            var buffer = new byte[512];
                            var bytesRead = default(int);
                            while ((bytesRead = reader.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                                memstream.Write(buffer, 0, bytesRead);
                            data = memstream.ToArray();
                        }
                    }

                    inputStream.Close();
                    inputStream.Dispose();
                }
            }
        }
    }
}
