import axios from "axios";

axios.defaults.headers.common["X-CSRF-TOKEN"] = document.cookie.split("=")[1];

//const baseUrl = 'http://localhost:4810/Api';

export default {
    // ********************************************************************| Authintecations |***********************************************************

    login(bodyObjeect) {
        return axios.post(`/Security/loginUser`, bodyObjeect);
    },

    AddMuntisbl(bodyObjeect) {
        return axios.post(`/api/admin/Municipalities/Add`, bodyObjeect);
    },

    IsLoggedin() {
        return axios.get(`/Security/IsLoggedin`);
    },

    Logout() {
        return axios.post(`/Security/Logout`);
    },







    // ********************************************************************| Dictionaries |***********************************************************



    // PaymentMethodss
    GetPaymentMethods(pageNo, pageSize) {
        return axios.get(`api/admin/Dictionaries/PaymentMethods/Get?pageno=${pageNo}&pagesize=${pageSize}`);
    },

    GetAllPaymentMethods() {
        return axios.get(`api/admin/Dictionaries/PaymentMethods/GetAll`);
    },

    AddPaymentMethods(bodyObject) {
        return axios.post(`api/admin/Dictionaries/PaymentMethods/Add`, bodyObject);
    },

    EditPaymentMethods(bodyObject) {
        return axios.post(`api/admin/Dictionaries/PaymentMethods/Edit`, bodyObject);
    },

    DeletePaymentMethods(Id) {
        return axios.post(`api/admin/Dictionaries/${Id}/PaymentMethods/Delete`);
    },






    // Facilities
    GetFacilities(pageNo, pageSize, CityId) {
        return axios.get(`api/admin/Dictionaries/Facilities/Get?pageno=${pageNo}&pagesize=${pageSize}&CityId=${CityId}`);
    },

    GetAllFacilities(CityId) {
        return axios.get(`api/admin/Dictionaries/Facilities/GetById?CityId=${CityId}`);
    },

    AddFacilities(bodyObject) {
        return axios.post(`api/admin/Dictionaries/Facilities/Add`, bodyObject);
    },

    EditFacilities(bodyObject) {
        return axios.post(`api/admin/Dictionaries/Facilities/Edit`, bodyObject);
    },

    DeleteFacilities(Id) {
        return axios.post(`api/admin/Dictionaries/${Id}/Facilities/Delete`);
    },











    // ********************************************************************| Companies  |***********************************************************


    AddCompanies(bodyObject) {
        return axios.post(`api/admin/Companies/Add`, bodyObject);
    },

    EditCompanies(bodyObject) {
        return axios.post(`api/admin/Companies/Edit`, bodyObject);
    },

    GetCompaniesById(code) {
        return axios.get(`api/admin/Companies/GetById?code=${code}`);
    },

    GetCompanies(pageNo, pageSize,level, ById, byDate) {
        return axios.get(`api/admin/Companies/Get?pageNo=${pageNo}&pageSize=${pageSize}&level=${level}&ById=${ById}&byDate=${byDate}`);
    },

    DeleteCompanies(Id) {
        return axios.post(`api/admin/Companies/${Id}/Delete`);
    },

    RestePasswordCompanies(Id) {
        return axios.post(`api/admin/Companies/${Id}/RestePassword`);
    },

    DeactivateCompanies(Id) {
        return axios.post(`api/admin/Companies/${Id}/Deactivate`);
    },

    ActivateCompanies(Id) {
        return axios.post(`api/admin/Companies/${Id}/Activate`);
    },

    ChangeStatusCompanies(Id) {
        return axios.post(`api/admin/Companies/${Id}/ChangeStatus`);
    },



    // Attachemtn
    AddCompaniesAttahcment(bodyObject) {
        return axios.post(`api/admin/Companies/Attachments/Add`, bodyObject);
    },

    GetCompaniesAttachment(Id) {
        return axios.get(`api/admin/Companies/Attachments/Get?Id=${Id}`);
    },

    DeleteCompaniesAttachment(Id) {
        return axios.post(`api/admin/Companies/${Id}/Attachments/Delete`);
    },


    //Companies  Wallet Info
    GetCompaniesWalletInfo(Id) {
        return axios.get(`api/admin/Companies/Wallet/Get?Id=${Id}`);
    },

    RechargeWallet(bodyObject) {
        return axios.post('/Api/Admin/Companies/RechargeWallet', bodyObject);
    },

    DeleteWalletTransacitons(Id) {
        return axios.post(`/Api/Admin/Companies/${Id}/DeletetWalletTransacitons`);
    },








    //Subscriptions
    GetSubscriptions(pageNo, pageSize) {
        return axios.get(`api/admin/Companies/Subscriptions/Get?pageNo=${pageNo}&pageSize=${pageSize}`);
    },









    //Offers
    GetOffers(pageNo, pageSize) {
        return axios.get(`api/admin/Companies/Offers/Get?pageNo=${pageNo}&pageSize=${pageSize}`);
    },

    AddOffers(bodyObject) {
        return axios.post(`/Api/Admin/Companies/Offers/Add`, bodyObject);
    },

    GetOffersRequest(pageNo, pageSize) {
        return axios.get(`api/admin/Companies/Offers/GetRequest?pageNo=${pageNo}&pageSize=${pageSize}`);
    },

    DeleteOffers(id) {
        return axios.post(`/Api/Admin/Companies/${id}/Offers/Delete`);
    },

    DeactivateOffers(id) {
        return axios.post(`/Api/Admin/Companies/${id}/Offers/Deactivate`);
    },

    ActiveOffers(id) {
        return axios.post(`/Api/Admin/Companies/${id}/Offers/Active`);
    },

    AcceptOffers(id) {
        return axios.post(`/Api/Admin/Companies/${id}/Offers/Accept`);
    },

    RejectOffers(id) {
        return axios.post(`/Api/Admin/Companies/${id}/Offers/Reject`);
    },







    // CompaniesRooms
    AddCompaniesRooms(bodyObject) {
        return axios.post(`/Api/Admin/Companies/CompaniesRooms/Add`, bodyObject);
    },

    GetCompaniesRooms() {
        return axios.get(`/Api/Admin/Companies/CompaniesRooms/Get`);
    },

    DeleteCompaniesRooms(Id) {
        return axios.post(`api/admin/Companies/${Id}/CompaniesRooms/Delete`);
    },




    //********************************************************************| Contact Us Info |***********************************************************

    GetContactUs(pageNo, pageSize) {
        return axios.get(`api/admin/Dictionaries/ContactUs/Get?pageNo=${pageNo}&pagesize=${pageSize}`);
    },









    // ********************************************************************| Users |***********************************************************
    GetUsers(pageNo, pageSize, UserType, PortId) {
        return axios.get(`api/admin/User/Get?pageNo=${pageNo}&pagesize=${pageSize}&userType=${UserType}&PortId=${PortId}`);
    },

    AddUser(bodyObject) {
        return axios.post("api/admin/User/Add", bodyObject);
    },

    EditUser(bodyObject) {
        return axios.post("api/admin/User/Edit", bodyObject);
    },

    ChangeStatusUser(Id) {
        return axios.post(`api/admin/User/${Id}/ChangeStatus`);
    },

    RestePassword(Id) {
        return axios.post(`api/admin/User/${Id}/RestePassword`);
    },

    DeleteUser(Id) {
        return axios.post(`api/admin/User/${Id}/Delete`);
    },

    UploadImage(bodyObject) {
        return axios.post("/Api/Admin/User/UploadImage", bodyObject);
    },

    EditUsersProfile(bodyObject) {
        return axios.post("/Api/Admin/User/EditUsersProfile", bodyObject);
    },

    ChangePassword(userPassword) {
        return axios.post(`/Api/Admin/User/ChangePassword`, userPassword);
    },

















    GetDashboardInfo() {
        return axios.get(`api/admin/Prisoners/GetDashboardInfo`);
    },






















};
