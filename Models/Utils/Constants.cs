using System.Collections.Generic;

namespace Models.Utils
{
   public class _MESSAGES
    {
        public static string unAuthorize= "You're not authorized please login again";
        public static string unAvailable = "You're not allowed to perform this action";


        //Auth
        public static string registered = "Registered successfully";
        public static string emailExists = "Email must be uniquie";
        public static string userNameExists = "Username must be uniquie";
        public static string incorrectCredentials = "Incorrect credentials ";
        public static string errorInSendingEmail = "Error occured while sending email";
        public static string emailSent = "Please check your email";
        public static string passwordUpdatefailed = "Error occured while updating password";
        public static string incorrectCode = "Incorrect code";
        public static string passwordUpdated = "Password updated successfully";



        //Stripe
        public static string subscribed = "Subscribed successfully";

        //Generic
        public static string success = "Success";
        public static string noRecordExist = "Record not found";
        public static string recordUpdated = "Record updated successfully";
        public static string recordDeleted = "Record deleted successfully";
        public static string errorInDeletingRecord = "Error occured while deleting record";

        //Order
        public static string orderPlaced = "Order placed successfully";
        

    }

    public class _SYSCONSTS
    {
        //Roles
        public static string[] roleSeller = {"1", "SELLER" };
        public static string[] roleBuyer   = { "2","BUYER" };

        

        //Permissions
        public static string[] buyerPermissions = { "4"};
        public static string[] sellerPermissions = {"1","2","3","4"};
    }
    
}
