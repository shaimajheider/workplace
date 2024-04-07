import axios from 'axios';

axios.defaults.headers.common['X-CSRF-TOKEN'] = document.cookie.split("=")[1];

//const baseUrl = 'http://localhost:4810/Api';

export default {


    //******************************************** Fince Student Info ********************************************
    RegesterApp(bodyObjeect) {
        return axios.post(`/Api/Admin/Helper/Regester`, bodyObjeect);
    },

    //******************************************** Dicanry Info ********************************************

    GetCities() {
        return axios.get(`/Api/Admin/Helper/Cities/GetAll`);
    },

    GetFacilities(CityId) {
        return axios.get(`/Api/Admin/Helper/Facilities/GetAll?CityId=${CityId}`);
    },


    //******************************************** Countact Us ********************************************
    CountactUs(bodyObjeect) {
        return axios.post(`/Api/Admin/Helper/CountactUs`, bodyObjeect);
    },

}