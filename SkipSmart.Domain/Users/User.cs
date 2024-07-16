using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Users.Events;

namespace SkipSmart.Domain.Users;

public class User : Entity {
     public FirstName FirstName { get; private set; }
     public LastName LastName { get; private set; }
     public Email Email { get; private set; }
     public Password Password { get; private set; }
     
     public int Subgroup { get; private set; }
     
     public bool IsEmailVerified { get; private set; }
     public EmailVerificationCode EmailVerificationCode { get; set; }
     public DateTime EmailVerificationSentAt { get; set; }
     
     public Guid GroupId { get; private set; }
     
     private User(Guid id, FirstName firstName, LastName lastName, Email email, int subgroup, bool isEmailVerified, Password password, Guid groupId)
         : base(id) 
     {
         FirstName = firstName;
         LastName = lastName;
         Email = email;
         Subgroup = subgroup;
         Password = password;
         GroupId = groupId;
         IsEmailVerified = isEmailVerified;
     }
     
     public static User Create(FirstName firstName, LastName lastName, Email email, int subgroup, Password password, Guid groupId) {
         var user = new User(Guid.NewGuid(), firstName, lastName, email, subgroup, false, password, groupId);
         
         user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
         
         return user;
     }
     
     public Result VerifyEmail() {
         IsEmailVerified = true;
         
         RaiseDomainEvent(new UserEmailVerifiedDomainEvent(Id));

         return Result.Success();
     }
     
     public void ChangeGroup(Guid newGroupId) {
         GroupId = newGroupId;
     }

     public void ChangeSubgroup(int newSubgroup) {
         Subgroup = newSubgroup;
     }
}