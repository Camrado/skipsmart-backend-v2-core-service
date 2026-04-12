using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Users.Events;

namespace SkipSmart.Domain.Users;

public class User : Entity {
     public string FirstName { get; private set; }
     public string LastName { get; private set; }
     public string Email { get; private set; }
     public Password Password { get; private set; }
     
     public int LanguageSubgroup { get; private set; }
     
     public int FacultySubgroup { get; private set; }
     
     public Guid GroupId { get; private set; }
     
     private User(Guid id, string firstName, string lastName, string email, int languageSubgroup, int facultySubgroup, Password password, Guid groupId)
         : base(id) 
     {
         FirstName = firstName;
         LastName = lastName;
         Email = email;
         LanguageSubgroup = languageSubgroup;
         FacultySubgroup = facultySubgroup;
         Password = password;
         GroupId = groupId;
     }
     
     private User() {
     }
     
     public static User Create(Guid userId, string firstName, string lastName, string email, int languageSubgroup, int facultySubgroup, Password password, Guid groupId) {
         var user = new User(userId, firstName, lastName, email, languageSubgroup, facultySubgroup, password, groupId);
         
         user.RaiseDomainEvent(new UserCreatedDomainEvent(userId));
         
         return user;
     }
     
     public void ChangeGroup(Guid newGroupId) {
         GroupId = newGroupId;
     }

     public void ChangeLanguageSubgroup(int newLanguageSubgroup) {
         LanguageSubgroup = newLanguageSubgroup;
     }
     
     public void ChangeFacultySubgroup(int newFacultySubgroup) {
         FacultySubgroup = newFacultySubgroup;
     }
}