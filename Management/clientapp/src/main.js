import Vue from 'vue';
import VueI18n from 'vue-i18n'
import VueRouter from 'vue-router';
import ElementUI from 'element-ui';
import Vuetify from 'vuetify'
import locale from 'element-ui/lib/locale/lang/en'
import BlockUIService from './Shared/BlockUIService.js';
import App from './App.vue';
import Layout from './components/Layout/Layout.vue';
import Login from './components/Login/Login.vue';
import Suspend from './components/Suspend/Suspend.vue';
//import Home from './components/Home/Home.vue';
import DataService from './Shared/DataService';
import Helper from './Shared/Helper';

import Users from './components/Users/Users.vue';
import Profile from './components/Users/EditUsersProfile/EditUsersProfile.vue';




import Clints from './components/Clints/Clints.vue';


import Companies from './components/Companies/Companies.vue';
import AddCompanies from './components/Companies/Add/Add.vue';
import CompaniesRequest from './components/Companies/Request/Request.vue';

import Subscriptions from './components/Companies/Subscriptions/Subscriptions.vue';

import Offers from './components/Companies/Offers/Offers.vue';
import OffersRequest from './components/Companies/Offers/Request/Request.vue';
import AddOffers from './components/Companies/Offers/Add/Add.vue';
import CompaniesRooms from './components/Companies/CompaniesRooms/CompaniesRooms.vue';
import AddCompaniesRooms from './components/Companies/CompaniesRooms/Add/Add.vue';


import PaymentMethods from './components/Dictionaries/PaymentMethods/PaymentMethods.vue';


import ContactUs from './components/ContactUs/ContactUs.vue';



import VueEllipseProgress from 'vue-ellipse-progress';

Vue.use(VueEllipseProgress);

Vue.use(Vuetify);
Vue.use(VueI18n);
Vue.use(VueRouter);
Vue.use(ElementUI, { locale });

Vue.config.productionTip = false;

Vue.prototype.$http = DataService;
Vue.prototype.$blockUI = BlockUIService;
Vue.prototype.$helper = Helper;

export const eventBus = new Vue();


const router = new VueRouter({
    mode: "history",
    base: __dirname,
    linkActiveClass: "active",
    routes: [
        {
            path: "/Login",
            component: Login,
        },
        {
            path: "/",
            component: App,
            children: [
                {
                    path: "",
                    component: Layout,
                    children: [
                        //{ path: "", component: Home },
                        { path: "", component: Subscriptions },

                        { path: "Users", component: Users },
                        { path: "Profile", component: Profile },

                        { path: "Suspend", component: Suspend },


                        { path: "Clints", component: Clints },

                        { path: "Companies", component: Companies },
                        { path: "AddCompanies", component: AddCompanies },
                        { path: "CompaniesRequest", component: CompaniesRequest },

                        { path: "Subscriptions", component: Subscriptions },

                        { path: "Offers", component: Offers },
                        { path: "OffersRequest", component: OffersRequest },
                        { path: "AddOffers", component: AddOffers },
                        { path: "CompaniesRooms", component: CompaniesRooms },
                        { path: "AddCompaniesRooms", component: AddCompaniesRooms },


                        { path: "PaymentMethods", component: PaymentMethods },

                        

                        { path: "ContactUs", component: ContactUs },



                        
                    ],
                },
            ],
        },
    ],
});

Vue.filter("toUpperCase", function (value) {
    if (!value) return "";
    return value.toUpperCase();
});

new Vue({
    router,
    render: (h) => {
        return h(App);
    },
}).$mount("#cpanel-management");
