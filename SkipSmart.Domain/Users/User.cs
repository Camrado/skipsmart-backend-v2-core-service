using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Users.Events;

namespace SkipSmart.Domain.Users;

public class User : Entity {
     public FirstName FirstName { get; private set; }
     public LastName LastName { get; private set; }
     public Email Email { get; private set; }
     public Password Password { get; private set; }
     
     public EmailVerificationCode EmailVerificationCode { get; set; }
     public EmailVerificationSentAt EmailVerificationSentAt { get; set; }
     
     public Guid GroupId { get; private set; }
     
     private User(Guid id, FirstName firstName, LastName lastName, Email email, Password password, Guid groupId)
         : base(id) 
     {
         FirstName = firstName;
         LastName = lastName;
         Email = email;
         Password = password;
         GroupId = groupId;
     }
     
     public static User Create(FirstName firstName, LastName lastName, Email email, Password password, Guid groupId) {
         var user = new User(Guid.NewGuid(), firstName, lastName, email, password, groupId);
         
         user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
         
         return user;
     }
}