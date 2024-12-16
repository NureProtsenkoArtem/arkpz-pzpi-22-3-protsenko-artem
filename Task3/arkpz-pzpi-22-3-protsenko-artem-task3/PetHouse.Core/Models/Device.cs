using System.Reflection.Metadata.Ecma335;
 using PetHouse.Core.Enums.Device;
 
 namespace PetHouse.Core.Models;
 
 public class Device
 {
    public Guid DeviceId { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string Model { get; set; }
    public DeviceStatus DeviceStatus { get; set; } = DeviceStatus.Offline;
    public FeedingMode FeedingMode { get; set; } = FeedingMode.Pause;
    public bool RecognitionEnabled { get; set; } = false;
    public bool CameraEnabled { get; set; } = false;
 }