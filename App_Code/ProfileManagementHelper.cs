using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


using System.Web.Profile;


namespace ProfileHelper
{
    /// <summary>
    /// Summary description for ProfileManagementHelper
    /// </summary>
    public class ProfileManagementHelper
    {
        string propertyDepth = "";

        public ProfileManagementHelper()
        {
            //  
            // TODO: Add constructor logic here
            //
        }

        #region method for set profile
        
        /// <summary>
        /// set the profile dictionary according to merchant_group and user_name 
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="username"></param>
        /// <param name="userProfile"></param>
        /// <returns>return "success" if profile is inserted successfully otherwise return "error!![error_text]"</returns>
        public string SetProfile(string groupName,string userName,Dictionary<string,object> userProfile)
        {
            if(Membership.GetUser(userName) == null)
                Membership.CreateUser(userName,"1234567");
            try
            {
                ProfileCommon pc = new ProfileCommon();
                ProfileCommon existing_pc = pc.GetProfile(userName);
                existing_pc.GetProfileGroup(groupName).SetPropertyValue("properties_block",userProfile);
                
                existing_pc.Save();
                
            }
            catch(Exception e)
            {
                return "error!!" + e;
            }
            return "success";
        }

        #endregion

        #region methods for get profile

        /// <summary>
        /// get all profiles of all groups for particular user 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>return a dictionary object that holds "group_name" as a key and total properties_block(dictionary object) for that particular group as a value</returns>
        public Dictionary<string,object> GetProfilesByUser(string userName)
        {
            Dictionary<string, object> profileListDictionary = new Dictionary<string, object>();
            
            ProfileCommon pc = new ProfileCommon();
            ProfileCommon existing_pc = pc.GetProfile(userName);
                        
            string []groupNameArray = Enum.GetNames(typeof(ProfileTypeEnum));
   
            for(int i=0;i<groupNameArray.Length;i++)
            {
                Dictionary<string,object> profileDictionary = (Dictionary<string,object>)existing_pc.GetProfileGroup(groupNameArray[i]).GetPropertyValue("properties_block");
                if(profileDictionary.Keys.Count > 0)
                    profileListDictionary.Add(groupNameArray[i],profileDictionary);
            }
            return profileListDictionary;
        }

        /// <summary>
        /// get all profiles of all groups with metioned properties for particular user 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="propertyNameList"></param>
        /// <returns>return a dictionary object that holds "group_name" as a key and total properties_block(dictionary object) for that particular group as a value</returns>
        public Dictionary<string,object> GetProfilesByUser(string userName,ArrayList propertyNameList)
        {
            Dictionary<string, object> profileListDictionary = new Dictionary<string, object>();
            ProfileCommon pc = new ProfileCommon();
            ProfileCommon existing_pc = pc.GetProfile(userName);
            
            string []groupNameArray = Enum.GetNames(typeof(ProfileTypeEnum));
   
            for(int i=0;i<groupNameArray.Length;i++)
            {
                Dictionary<string,object> profileDictionary = (Dictionary<string,object>)existing_pc.GetProfileGroup(groupNameArray[i]).GetPropertyValue("properties_block");
                if(profileDictionary.Keys.Count > 0)
                {
                    propertyDepth = "";
                    Dictionary<string, object> selectedPropertiesDictionary = new Dictionary<string,object>();
                    selectedPropertiesDictionary = FindProperties(profileDictionary,selectedPropertiesDictionary,propertyNameList);
             
                    if(selectedPropertiesDictionary.Keys.Count > 0)
                        profileListDictionary.Add(groupNameArray[i],selectedPropertiesDictionary);
                }
            }
            return profileListDictionary;
        }

        /// <summary>
        /// get profiles of all users for particular groupName 
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns>return a dictionary object that holds "user_name" as a key and total properties_block(dictionary object) for that particular user as a value</returns>
        public Dictionary<string,object> GetProfilesByGroup(string groupName)
        {
            Dictionary<string, object> profileListDictionary = new Dictionary<string, object>();
            ProfileCommon pc = new ProfileCommon();
           
            MembershipUserCollection userList = Membership.GetAllUsers();
            foreach(MembershipUser user in userList)
            {
                ProfileCommon existing_pc = pc.GetProfile(user.UserName);

                Dictionary<string,object> profileDictionary = (Dictionary<string,object>)existing_pc.GetProfileGroup(groupName).GetPropertyValue("properties_block");
                if(profileDictionary.Keys.Count > 0)
                    profileListDictionary.Add(user.UserName,profileDictionary);
            
            }
            
            return profileListDictionary;
        }


        /// <summary>
        /// get profiles of all users with metioned properties for particular groupName 
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="propertyNameList"></param>
        /// <returns>return a dictionary object that holds "user_name" as a key and total properties_block(dictionary object) for that particular user as a value</returns>
        public Dictionary<string,object> GetProfilesByGroup(string groupName,ArrayList propertyNameList)
        {
            Dictionary<string, object> profileListDictionary = new Dictionary<string, object>();
            ProfileCommon pc = new ProfileCommon();
           
            MembershipUserCollection userList = Membership.GetAllUsers();
            foreach(MembershipUser user in userList)
            {
                ProfileCommon existing_pc = pc.GetProfile(user.UserName);

                Dictionary<string,object> profileDictionary = (Dictionary<string,object>)existing_pc.GetProfileGroup(groupName).GetPropertyValue("properties_block");
                
                if(profileDictionary.Keys.Count > 0)
                {
                    propertyDepth = "";
                    Dictionary<string, object> selectedPropertiesDictionary = new Dictionary<string,object>();
                    selectedPropertiesDictionary = FindProperties(profileDictionary,selectedPropertiesDictionary,propertyNameList);
             
                    if(selectedPropertiesDictionary.Keys.Count > 0)
                        profileListDictionary.Add(user.UserName,selectedPropertiesDictionary);
                }
            
            }
            
            return profileListDictionary;
        }

        /// <summary>
        /// get profile for particular group and user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="groupName"></param>
        /// <returns>return a dictionary object that holds "property_name" as a key and "property_value" as a value</returns>
        public Dictionary<string,object> GetProfileByUserGroup(string userName,string groupName)
        {
            ProfileCommon pc = new ProfileCommon();
            ProfileCommon existing_pc = pc.GetProfile(userName);
              
            return (Dictionary<string,object>)existing_pc.GetProfileGroup(groupName).GetPropertyValue("properties_block");
           
        }

        /// <summary>
        /// get profile with metioned properties for particular group and user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="groupName"></param>
        /// <returns>return a dictionary object that holds "property_name" as a key and "property_value" as a value</returns>
        public Dictionary<string,object> GetProfileByUserGroup(string userName,string groupName,ArrayList propertyNameList)
        {
            Dictionary<string, object> selectedPropertiesDictionary = new Dictionary<string,object>();
            ProfileCommon pc = new ProfileCommon();
            ProfileCommon existing_pc = pc.GetProfile(userName);
            
            Dictionary<string,object> profileDictionary = (Dictionary<string,object>)existing_pc.GetProfileGroup(groupName).GetPropertyValue("properties_block");
                
            if(profileDictionary.Keys.Count > 0)
            {
                propertyDepth = "";
                selectedPropertiesDictionary = FindProperties(profileDictionary,selectedPropertiesDictionary,propertyNameList);
            }
            
            return selectedPropertiesDictionary;
        }

        /// <summary>
        /// Get all profiles for all user and group
        /// </summary>
        /// <returns>return a dictionary object that holds "user_name|merchant_Group_Name" as a key and total properties_block(dictionary object) for that particular user and group as a value</returns>
        public Dictionary<string,object> GetTotalProfileCollection()
        {
            Dictionary<string, object> profileListDictionary = new Dictionary<string, object>();
            MembershipUserCollection userList = Membership.GetAllUsers();
            foreach(MembershipUser user in userList)
            {
                ProfileCommon pc = new ProfileCommon();
                ProfileCommon existing_pc = pc.GetProfile(user.UserName);                
                string []groupNameArray = Enum.GetNames(typeof(ProfileTypeEnum));
       
                for(int i=0;i<groupNameArray.Length;i++)
                {
                    Dictionary<string,object> profileDictionary = (Dictionary<string,object>)existing_pc.GetProfileGroup(groupNameArray[i]).GetPropertyValue("properties_block");
                    if(profileDictionary.Keys.Count > 0)
                        profileListDictionary.Add(user.UserName+"|"+groupNameArray[i],profileDictionary);
                }
                
            }
            return profileListDictionary;
        }
        
        /// <summary>
        /// get only metioned properties of profile for all user and group
        /// </summary>
        /// <param name="propertyNameList"></param>
        /// <returns>return a dictionary object that holds "user_name|merchant_Group_Name" as a key and total properties_block(dictionary object) for that particular user and group as a value</returns>
        public Dictionary<string,object> GetTotalProfileCollection(ArrayList propertyNameList)
        {
            Dictionary<string, object> profileListDictionary = new Dictionary<string, object>();
            MembershipUserCollection userList = Membership.GetAllUsers();
            foreach(MembershipUser user in userList)
            {
                ProfileCommon pc = new ProfileCommon();
                ProfileCommon existing_pc = pc.GetProfile(user.UserName);
                
                string []groupNameArray = Enum.GetNames(typeof(ProfileTypeEnum));
       
                for(int i=0;i<groupNameArray.Length;i++)
                {
                    Dictionary<string,object> profileDictionary = (Dictionary<string,object>)existing_pc.GetProfileGroup(groupNameArray[i]).GetPropertyValue("properties_block");
                    if(profileDictionary.Keys.Count > 0)
                    {
                        propertyDepth = "";
                        Dictionary<string, object> selectedPropertiesDictionary = new Dictionary<string,object>();
                        selectedPropertiesDictionary = FindProperties(profileDictionary,selectedPropertiesDictionary,propertyNameList);
                 
                        if(selectedPropertiesDictionary.Keys.Count > 0)
                            profileListDictionary.Add(user.UserName+"|"+groupNameArray[i],selectedPropertiesDictionary);
                    }
                }
                
            }
            return profileListDictionary;
        }
        
        

        public Dictionary<string,object> FindProperties(Dictionary<string,object> profileDictionary,Dictionary<string, object> selectedPropertiesDictionary,ArrayList propertyNameList)
        {
            foreach(object key in profileDictionary.Keys)
            {
                object value = profileDictionary[key.ToString()];
                if(value.GetType().Equals(typeof(System.Collections.Generic.Dictionary<string,object>)))
                {
                    propertyDepth += key.ToString()+"|";
                    FindProperties((Dictionary<string, object>)value,selectedPropertiesDictionary,propertyNameList);
                    propertyDepth = propertyDepth.Substring(0,propertyDepth.LastIndexOf(key.ToString()+"|"));
                }
                else
                {
                    if(propertyNameList.Contains(key.ToString()))
                        selectedPropertiesDictionary.Add(propertyDepth + key.ToString(),profileDictionary[key.ToString()]);
                }
            }
            
            return selectedPropertiesDictionary;
        }

        #endregion

        #region methods for remove profile
        
        /// <summary>
        /// completely remove/delete the specific user's all type of profile
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool RemoveProfilesByUser(string userName)
        {
            return ProfileManager.DeleteProfile(userName);
        }

        /// <summary>
        /// just remove all the properties and values from particular user and group's profile but the empty instance exists  
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool RemoveProfileByUserGroup(string userName,string groupName)
        {
            ProfileCommon pc = new ProfileCommon();
            ProfileCommon existing_pc = pc.GetProfile(userName);
            try{
                Dictionary<string,object> profile = (Dictionary<string,object>)existing_pc.GetProfileGroup(groupName).GetPropertyValue("properties_block");  
                profile.Clear();
                if(!SetProfile(groupName,userName,profile).Equals("success"))
                    return false;
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
              
        }

        /// <summary>
        /// remove only provided properties from particular user and group's profile 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="groupName"></param>
        /// <param name="propertyNameList"></param>
        /// <returns></returns>
        public bool RemoveProfilePropertiesByUserGroup(string userName,string groupName,ArrayList propertyNameList)
        {
            Dictionary<string, object> existPropertiesDictionary = new Dictionary<string,object>();
            ProfileCommon pc = new ProfileCommon();
            ProfileCommon existing_pc = pc.GetProfile(userName);
            
            Dictionary<string,object> profileDictionary = (Dictionary<string,object>)existing_pc.GetProfileGroup(groupName).GetPropertyValue("properties_block");
                
            if(profileDictionary.Keys.Count > 0)
            {
                propertyDepth = "";
                existPropertiesDictionary = DeleteProperties(profileDictionary,propertyNameList);
            }
            
            if(!SetProfile(groupName,userName,existPropertiesDictionary).Equals("success"))
                 return false;

            return true;
        }

        public Dictionary<string,object> DeleteProperties(Dictionary<string,object> profileDictionary,ArrayList propertyNameList)
        {
            string []keyCollection = new string[profileDictionary.Keys.Count];
            int j=0;
            foreach(object key in profileDictionary.Keys)
                keyCollection[j++] = key.ToString();

            for(int i=0;i<keyCollection.Length;i++)
            {
                string key = keyCollection[i];
                object value = profileDictionary[key];

                if(propertyNameList.Contains(key))
                        profileDictionary.Remove(key);
                else if(value.GetType().Equals(typeof(System.Collections.Generic.Dictionary<string,object>)))
                {
                    propertyDepth += key+"|";
                    DeleteProperties((Dictionary<string, object>)value,propertyNameList);
                    propertyDepth = propertyDepth.Substring(0,propertyDepth.LastIndexOf(key.ToString()+"|"));
                }
            }
            
            return profileDictionary;
        }
        
        #endregion  
    }

    public enum ProfileTypeEnum
    {
        UsersPersonalInfo = 1,
        UsersOfficialInfo
    }
}
