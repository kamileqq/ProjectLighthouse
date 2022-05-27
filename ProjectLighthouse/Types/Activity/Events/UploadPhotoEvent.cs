using LBPUnion.ProjectLighthouse.Serialization;

namespace LBPUnion.ProjectLighthouse.Types.Activity.Events;

public class UploadPhotoEvent : IEvent
{
    public long Timestamp { get; set; }
    public User User { get; init; }
    public Photo Photo { get; init; }
    public string Serialize()
    {
        string @event = LbpSerializer.StringElement("timestamp", this.Timestamp) +
                        LbpSerializer.StringElement("actor", this.User.Username) +
                        LbpSerializer.TaggedStringElement("object_slot_id", 1, "type", "user") +
                        LbpSerializer.StringElement("photo_id", this.Photo.PhotoId);

        return LbpSerializer.TaggedStringElement("event", @event, "type", "upload_photo");
    }
}