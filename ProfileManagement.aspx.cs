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


using ProfileHelper;

public partial class ProfileManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
            LoadAll();
    }

    public void LoadAll()
    {
        LoadProfileTypeList();
    }

    public void ReloadAll()
    {
        lblErrorMessage.Text = "";
        lblReportInfo.Text = "";
    }

    public void LoadProfileTypeList()
    {
        foreach (ProfileTypeEnum profileType in Enum.GetValues(typeof(ProfileTypeEnum)))
        {
            ddlProfileList.Items.Add(new ListItem(profileType.ToString(), Convert.ToInt32(profileType).ToString()));
        }
    }

    #region event methods for add user provided properties to list
    
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if(!txtPropertyName.Text.Equals(""))
        {
            lstProperties.Items.Add(new ListItem(txtPropertyName.Text,txtPropertyValue.Text));
        }
    }

    protected void btnRemove_Click(object sender, EventArgs e)
     {
        if(lstProperties.Items.Count > 0)
        {
            if(lstProperties.SelectedIndex < 0)
                lstProperties.Items.RemoveAt(lstProperties.Items.Count-1);
            else
                lstProperties.Items.RemoveAt(lstProperties.SelectedIndex);
        }
    }
    #endregion

    #region event methods for set profile

    protected void btnSetProfile_Click(object sender, EventArgs e)
    {
        ReloadAll();
        ProfileManagementHelper pmh = new ProfileManagementHelper();
        
        if(!txtUserName.Text.Equals(""))
        {
            string message = pmh.SetProfile(ddlProfileList.SelectedItem.Text,txtUserName.Text,FillPropertiesToDictionary());
            if(message.Equals("success"))
                lblErrorMessage.Text = "profile is succesfully inserted";
            else
                lblErrorMessage.Text = message;
        }
        else
            lblErrorMessage.Text = "please insert the user name";
    }

    public Dictionary<string, object> FillPropertiesToDictionary()
    {
        Dictionary<string, object> profileDictionary = new Dictionary<string, object>();
        
        foreach(ListItem item in lstProperties.Items)
        {
            profileDictionary.Add(item.Text,item.Value);
        }

        return profileDictionary;
    }

    #endregion

    #region event methods for retrive profile

    protected void btnGetProfile_Click(object sender, EventArgs e)
    {
        switch(rdoRetriveOptions.SelectedValue)
        {
            case "1" :
                RetriveProfileByUser();
                break;
            case "2" :
                RetriveProfileByUserPropertyList();
                break;
            case "3" :
                RetriveProfileByGroup();
                break;
            case "4" :
                RetriveProfileByGroupPropertyList();
                break;
            case "5" :
                RetriveProfileByUserGroup();
                break;
            case "6" :
                RetriveProfileByUserGroupPropertyList();
                break;
            case "7" :
                RetriveAllProfiles();
                break;
            case "8" :
                RetriveAllProfilesByPropertyList();
                break;
        }

        if(lblReportInfo.Text.Equals(""))
            lblReportInfo.Text = "sorry, there is no available information.";
    }

    public void RetriveProfileByUser()
    {
        ReloadAll();
        ProfileManagementHelper pmh = new ProfileManagementHelper();
        
        if(!txtUserName.Text.Equals(""))
        {
            Dictionary<string, object> reportContainerDictionary = pmh.GetProfilesByUser(txtUserName.Text);
            
            foreach(string groupName in reportContainerDictionary.Keys)
            {
                lblReportInfo.Text += "User Name : " + txtUserName.Text + "<br />"; 
                lblReportInfo.Text += "Profile Type : " + groupName + "<br />";
                lblReportInfo.Text += "Property List : " + "<br />";
                
                Dictionary<string, object> propertyListDictionary = ((Dictionary<string, object>)reportContainerDictionary[groupName]);
                foreach(string propertyName in propertyListDictionary.Keys)
                {
                    lblReportInfo.Text += propertyName + "&nbsp;&nbsp;&nbsp;&nbsp;" + propertyListDictionary[propertyName].ToString() + "<br />"; 
                }

                lblReportInfo.Text += "<br /><br />";
            }
        }
        else
            lblErrorMessage.Text = "please insert the user name";

    }

    public void RetriveProfileByUserPropertyList()
    {
        ReloadAll();
        ProfileManagementHelper pmh = new ProfileManagementHelper();
        
        if(!txtUserName.Text.Equals(""))
        {
           
            Dictionary<string, object> reportContainerDictionary = pmh.GetProfilesByUser(txtUserName.Text,GetUserProvidedPropertyList());
            
            foreach(string groupName in reportContainerDictionary.Keys)
            {
                lblReportInfo.Text += "User Name : " + txtUserName.Text + "<br />"; 
                lblReportInfo.Text += "Profile Type : " + groupName + "<br />";
                lblReportInfo.Text += "Property List : " + "<br />";
                
                Dictionary<string, object> propertyListDictionary = ((Dictionary<string, object>)reportContainerDictionary[groupName]);
                foreach(string propertyName in propertyListDictionary.Keys)
                {
                    lblReportInfo.Text += propertyName + "&nbsp;&nbsp;&nbsp;&nbsp;" + propertyListDictionary[propertyName].ToString() + "<br />"; 
                }

                lblReportInfo.Text += "<br /><br />";
            }
        }
        else
            lblErrorMessage.Text = "please insert the user name";
    }

    public void RetriveProfileByGroup()
    {
        ReloadAll();
        ProfileManagementHelper pmh = new ProfileManagementHelper();
        
        
        Dictionary<string, object> reportContainerDictionary = pmh.GetProfilesByGroup(ddlProfileList.SelectedItem.Text);
        
        foreach(string userName in reportContainerDictionary.Keys)
        {
            lblReportInfo.Text += "User Name : " + userName + "<br />"; 
            lblReportInfo.Text += "Profile Type : " + ddlProfileList.SelectedItem.Text + "<br />";
            lblReportInfo.Text += "Property List : " + "<br />";
            
            Dictionary<string, object> propertyListDictionary = ((Dictionary<string, object>)reportContainerDictionary[userName]);
            foreach(string propertyName in propertyListDictionary.Keys)
            {
                lblReportInfo.Text += propertyName + "&nbsp;&nbsp;&nbsp;&nbsp;" + propertyListDictionary[propertyName].ToString() + "<br />"; 
            }

            lblReportInfo.Text += "<br /><br />";
        }
        

    }

    public void RetriveProfileByGroupPropertyList()
    {
        ReloadAll();
        ProfileManagementHelper pmh = new ProfileManagementHelper();
        
        Dictionary<string, object> reportContainerDictionary = pmh.GetProfilesByGroup(ddlProfileList.SelectedItem.Text,GetUserProvidedPropertyList());
        
        foreach(string userName in reportContainerDictionary.Keys)
        {
            lblReportInfo.Text += "User Name : " + userName + "<br />"; 
            lblReportInfo.Text += "Profile Type : " + ddlProfileList.SelectedItem.Text + "<br />";
            lblReportInfo.Text += "Property List : " + "<br />";
            
            Dictionary<string, object> propertyListDictionary = ((Dictionary<string, object>)reportContainerDictionary[userName]);
            foreach(string propertyName in propertyListDictionary.Keys)
            {
                lblReportInfo.Text += propertyName + "&nbsp;&nbsp;&nbsp;&nbsp;" + propertyListDictionary[propertyName].ToString() + "<br />"; 
            }

            lblReportInfo.Text += "<br /><br />";
        }
        
    }

    public void RetriveProfileByUserGroup()
    {
        ReloadAll();
        ProfileManagementHelper pmh = new ProfileManagementHelper();
        
        if(!txtUserName.Text.Equals(""))
        {
            Dictionary<string, object> reportContainerDictionary = pmh.GetProfileByUserGroup(txtUserName.Text,ddlProfileList.SelectedItem.Text);
           
            if(reportContainerDictionary.Keys.Count != 0)
            {
                lblReportInfo.Text += "User Name : " + txtUserName.Text + "<br />"; 
                lblReportInfo.Text += "Profile Type : " + ddlProfileList.SelectedItem.Text + "<br />";
                lblReportInfo.Text += "Property List : " + "<br />";
                
                foreach(string propertyName in reportContainerDictionary.Keys)
                {
                    lblReportInfo.Text += propertyName + "&nbsp;&nbsp;&nbsp;&nbsp;" + reportContainerDictionary[propertyName].ToString() + "<br />"; 
                }

                lblReportInfo.Text += "<br /><br />";
            }
        }
        else
            lblErrorMessage.Text = "please insert the user name";
    }

    public void RetriveProfileByUserGroupPropertyList()
    {
        ReloadAll();
        ProfileManagementHelper pmh = new ProfileManagementHelper();
        
        if(!txtUserName.Text.Equals(""))
        {
            Dictionary<string, object> reportContainerDictionary = pmh.GetProfileByUserGroup(txtUserName.Text,ddlProfileList.SelectedItem.Text,GetUserProvidedPropertyList());
            
            if(reportContainerDictionary.Keys.Count != 0)
            {
                lblReportInfo.Text += "User Name : " + txtUserName.Text + "<br />"; 
                lblReportInfo.Text += "Profile Type : " + ddlProfileList.SelectedItem.Text + "<br />";
                lblReportInfo.Text += "Property List : " + "<br />";
                
                foreach(string propertyName in reportContainerDictionary.Keys)
                {
                    lblReportInfo.Text += propertyName + "&nbsp;&nbsp;&nbsp;&nbsp;" + reportContainerDictionary[propertyName].ToString() + "<br />"; 
                }

                lblReportInfo.Text += "<br /><br />";
            }
        }
        else
            lblErrorMessage.Text = "please insert the user name";
    }

    public void RetriveAllProfiles()
    {
        ReloadAll();
        ProfileManagementHelper pmh = new ProfileManagementHelper();
        
        Dictionary<string, object> reportContainerDictionary = pmh.GetTotalProfileCollection();
        
        foreach(string userGroupName in reportContainerDictionary.Keys)
        {
            lblReportInfo.Text += "User Name : " + userGroupName.Split('|')[0] + "<br />"; 
            lblReportInfo.Text += "Profile Type : " + userGroupName.Split('|')[1] + "<br />";
            lblReportInfo.Text += "Property List : " + "<br />";
            
            Dictionary<string, object> propertyListDictionary = ((Dictionary<string, object>)reportContainerDictionary[userGroupName]);
            foreach(string propertyName in propertyListDictionary.Keys)
            {
                lblReportInfo.Text += propertyName + "&nbsp;&nbsp;&nbsp;&nbsp;" + propertyListDictionary[propertyName].ToString() + "<br />"; 
            }

            lblReportInfo.Text += "<br /><br />";
        }
            
        
    }

    public void RetriveAllProfilesByPropertyList()
    {
        ReloadAll();
        ProfileManagementHelper pmh = new ProfileManagementHelper();
        
        Dictionary<string, object> reportContainerDictionary = pmh.GetTotalProfileCollection(GetUserProvidedPropertyList());
        
        foreach(string userGroupName in reportContainerDictionary.Keys)
        {
            lblReportInfo.Text += "User Name : " + userGroupName.Split('|')[0] + "<br />"; 
            lblReportInfo.Text += "Profile Type : " + userGroupName.Split('|')[1] + "<br />";
            lblReportInfo.Text += "Property List : " + "<br />";
            
            Dictionary<string, object> propertyListDictionary = ((Dictionary<string, object>)reportContainerDictionary[userGroupName]);
            foreach(string propertyName in propertyListDictionary.Keys)
            {
                lblReportInfo.Text += propertyName + "&nbsp;&nbsp;&nbsp;&nbsp;" + propertyListDictionary[propertyName].ToString() + "<br />"; 
            }

            lblReportInfo.Text += "<br /><br />";
        }


    }

    #endregion

    #region event methods for remove profile

    protected void btnRemoveProfile_Click(object sender, EventArgs e)
    {
        switch(rdoRemoveOptions.SelectedValue)
        {
            case "1" :
                RemoveAllProfilesByUser();
                break;
            case "2" :
                RemoveAllProfilePropertiesByUserGroup();
                break;
            case "3" :
                RetrivePropertiesByUserGroupPropertyList();
                break;
        }
    }

    public void RemoveAllProfilesByUser()
    {
        ReloadAll();
        ProfileManagementHelper pmh = new ProfileManagementHelper();
        
        if(!txtUserName.Text.Equals(""))
        {
            if(pmh.RemoveProfilesByUser(txtUserName.Text))
            {
                lblErrorMessage.Text = "remove " +txtUserName.Text+ "'s profile successfully";
            }
            else
                lblErrorMessage.Text = "remove profile unsuccessful";
            
        }
        else
            lblErrorMessage.Text = "please insert the user name";
    }

    public void RemoveAllProfilePropertiesByUserGroup()
    {
        ReloadAll();
        ProfileManagementHelper pmh = new ProfileManagementHelper();
        
        if(!txtUserName.Text.Equals(""))
        {
            if(pmh.RemoveProfileByUserGroup(txtUserName.Text,ddlProfileList.SelectedItem.Text))
            {
                lblErrorMessage.Text = "remove " +txtUserName.Text+ "'s "+ ddlProfileList.SelectedItem.Text +" profile successfully";
            }
            else
                lblErrorMessage.Text = "remove profile unsuccessful";
            
        }
        else
            lblErrorMessage.Text = "please insert the user name";
    }

    public void RetrivePropertiesByUserGroupPropertyList()
    {
        ReloadAll();
        ProfileManagementHelper pmh = new ProfileManagementHelper();
        
        if(!txtUserName.Text.Equals(""))
        {
            if(pmh.RemoveProfilePropertiesByUserGroup(txtUserName.Text,ddlProfileList.SelectedItem.Text,GetUserProvidedPropertyList()))
            {
                lblErrorMessage.Text = "remove all the metioned properties of " +txtUserName.Text+ "'s "+ ddlProfileList.SelectedItem.Text +" profile successfully";
            }
            else
                lblErrorMessage.Text = "remove profile unsuccessful";
            
        }
        else
            lblErrorMessage.Text = "please insert the user name";
    }

    #endregion

    #region helper method for fillup an array list with user provided properties 
    
    public ArrayList GetUserProvidedPropertyList()
    {
        ArrayList propertyList = new ArrayList();

        foreach(ListItem item in lstProperties.Items)
        {
            propertyList.Add(item.Text);
        }

        return propertyList;
    }

    #endregion
    
}
