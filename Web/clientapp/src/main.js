import Vue from 'vue';
import VueI18n from 'vue-i18n';
import VueRouter from 'vue-router';
import ElementUI from 'element-ui';
import Vuetify from 'vuetify'
import locale from 'element-ui/lib/locale/lang/en'
import BlockUIService from './Shared/BlockUIService.js';
import App from './App.vue';
import Layout from './components/Layout/Layout.vue';

import Home from './components/Home/Home.vue';
import DataService from './Shared/DataService';
import Helper from './Shared/Helper';

import CommingSoon from './components/WebPage/CommingSoon/CommingSoon.vue';
import ContactUs from './components/WebPage/ContactUs/ContactUs.vue';
//import AboutUs from './components/WebPage/AboutUs/AboutUs.vue';

//import Login from './components/Authentications/Login.vue';
//import Evercookie from './components/Authentications/utils/evercookie'
//import Login from './components/Authentications/Login.vue';
import SignUp from './components/Authentications/Sinup/Sinup.vue';

//import Courses from './components/Courses/Courses.vue';
//import CourseInfo from './components/Courses/Info/Info.vue';
//import Checkout from './components/Courses/Checkout/Checkout.vue';

//import Students from './components/Students/Students.vue';
//import StudentsCoures from './components/Students/Courses/Courses.vue';
//import StudentsCoursesNew from './components/Students/CoursesNew/Courses.vue';

//import Instructors from './components/Instructors/Instructors.vue';
//import InstructorProfile from './components/Instructors/InstructorProfile/InstructorProfile.vue';



import VueEllipseProgress from 'vue-ellipse-progress';

Vue.use(VueEllipseProgress);

Vue.use(Vuetify)
Vue.use(VueI18n);
Vue.use(VueRouter);
//Vue.use(Evercookie)
Vue.use(ElementUI, { locale });

Vue.config.productionTip = false;
Vue.config.devtools = 'eval-source-map';
//Vue.config.parallel = false;

Vue.prototype.$http = DataService;
Vue.prototype.$blockUI = BlockUIService;
Vue.prototype.$helper = Helper;


export const eventBus = new Vue();

//export const config= new defineConfig({
//    parallel: false
//});

//const i18n = new VueI18n({
//    locale: 'ar', // set locale
//    messages, // set locale messages
//})

const router = new VueRouter({
    mode: 'history',
    base: __dirname,
    linkActiveClass: 'active',
    routes: [
        //{
        //    //path: '/Login',
        //    //component: Login,
        //    path: '/StudentsCoures',
        //    //component: StudentsCoures,
        //    component: StudentsCoures,
         
        //},
        //{
        //    //path: '/Login',
        //    //component: Login,
        //    path: '/StudentsCoursesNew',
        //    //component: StudentsCoures,
        //    component: StudentsCoursesNew,
         
        //},
         {
            path: '/',
            component: App,
            children: [
                {
                    path: '',
                    component: Layout,
                    children: [
                        { path: '', component: Home },
                        //{ path: '', component: Sinup },
                        { path: 'CommingSoon', component: CommingSoon },
                        { path: 'ContactUs', component: ContactUs },
                        { path: 'SignUp', component: SignUp },
                        //{ path: 'AboutUs', component: AboutUs },
                        //{ path: 'Login', component: Login },
                        //{ path: 'Courses', component: Courses },
                        //{ path: 'CourseInfo', component: CourseInfo },
                        //{ path: 'Checkout', component: Checkout },
                        //{ path: 'Instructors', component: Instructors },
                        //{ path: 'InstructorProfile', component: InstructorProfile },
                        //{ path: 'Students', component: Students },
                       
                    ]
                },
            ],
        }
    ]
});

Vue.filter('toUpperCase', function (value) {
    if (!value) return '';
    return value.toUpperCase();
});

new Vue({
    router,
    render: h => {
        return h(App);
    }
}).$mount('#cpanel-management');
