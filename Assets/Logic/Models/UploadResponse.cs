public class UploadResponse
{
    public string audio_url;
    public AudioMetadata audio_metadata;
}
public class AudioMetadata
{
    public string id;
    public string filename;
    public string extension;
    public long size;
    public float audio_duration;
    public int number_of_channels;
}
