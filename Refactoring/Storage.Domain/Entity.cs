
using Flunt.Notifications;

namespace Refactoring.Models
{
  public class Entity:Notifiable
  {
    public Guid Id { get; private set; }
    public Entity()
    {
      Id = Guid.NewGuid();
    }
  }
}
