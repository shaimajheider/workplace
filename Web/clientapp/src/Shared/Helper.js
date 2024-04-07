




//import moment from 'moment';
import Swal from 'sweetalert2'
//import { FlatShading } from '../../public/assets/js/plugins/threejs';



export default (function () {
    return {


         //******************************************** Moment Info ********************************************
        //moment: function (date) {
        //    if (date === null) {
        //        return "فارغ";
        //    }
        //    // return moment(date).format('MMMM Do YYYY, h:mm:ss a');
        //    return moment(date).format('MMMM Do YYYY');
        //}




        //******************************************** User Info ********************************************

        CheckLoginStatus() {
            try {
                let loginDetails= JSON.parse(localStorage.getItem('currentUser-client'));
                if (this.loginDetails == null) {
                    window.location.href = '/Login';
                }
                return loginDetails;
            } catch (error) {
                window.location.href = '/Login';
            }
        },





        //******************************************** Card Step Info ********************************************
        nextStep(activeStep, formSteps) {
            if (activeStep < formSteps) {
                activeStep += 1;
                return activeStep;
            } else {
                activeStep -= 1;
                return activeStep;
            }

        },
        prevStep(activeStep) {
            if (activeStep > 0) {
                activeStep -= 1;
                return activeStep;
            }
        },









        //******************************************** Form Info ********************************************

        submitForm(formName) {
            let res;
            formName.validate((valid) => {
                if (valid) {
                    res = true;
                } else {
                    res = false;
                    this.showSwal('warning');
                }
            } );
            return res;
        },




        //******************************************** Message Info ********************************************
        showSwal(type) {
            if (type === "warning") {
                Swal.fire({
                    icon: 'warning',
                    title: 'عملية غير ناجحة',
                    html: 'الرجاء التحقق من ادخال جميع البيانات',
                    showCancelButton: false,
                }).then(() => {

                });
            }

        },

        ShowMessage(icon, title, html) {
            Swal.fire({
                icon: icon,
                title: title,
                html: html,
                showCancelButton: false,
            }).then(() => {

            });
        },






        //******************************************** Object Info ********************************************

        DynamicArabicEnterRequired(property) {
            return [
                { required: true, message: 'الرجاء التأكد من إدخال '+ property, trigger: 'blur' },
                { min: 3, max: 150, message: 'يجب ان يكون الطول من 3 الي 150', trigger: 'blur' },
                { required: true, pattern: /[\u0600-\u06FF]/, message: 'الرجاء إدخال حروف العربية فقط', trigger: 'blur' }
            ]
        },

        DynamicArabicSelectRequired(property) {
            return [
                { required: true, message: 'الرجاء التأكد من إختيار'+ property, trigger: 'blur' },
                { min: 3, max: 150, message: 'يجب ان يكون الطول من 3 الي 150', trigger: 'blur' },
                { required: true, pattern: /[\u0600-\u06FF]/, message: 'الرجاء إدخال حروف العربية فقط', trigger: 'blur' }
            ]
        },

        DynamicEnglishEnterRequired(property) {
            return [
                { required: true, message: 'الرجاء التأكد من إدخال ' + property, trigger: 'blur' },
                { min: 3, max: 150, message: 'يجب ان يكون الطول من 3 الي 150', trigger: 'blur' },
                { required: true, pattern: /^[a-zA-Z]+$/, message: 'الرجاء إدخال حروف إنجليزية فقط', trigger: 'blur' }
            ]
        },

        DynamicEnglishSelectRequired(property) {
            return [
                { required: true, message: 'الرجاء التأكد من إختيار'+ property, trigger: 'blur' },
                { min: 3, max: 150, message: 'يجب ان يكون الطول من 3 الي 150', trigger: 'blur' },
                { required: true, pattern: /^[a-zA-Z]+$/, message: 'الرجاء إدخال حروف إنجليزية فقط', trigger: 'blur' }
            ]
        },

        ArabicOnly() {
            return [
                { required: true, message: 'الرجاء تعبئة البيانات', trigger: 'blur' },
                { min: 3, max: 150, message: 'يجب ان يكون الطول من 3 الي 150', trigger: 'blur' },
                { required: true, pattern: /[\u0600-\u06FF]/, message: 'الرجاء إدخال حروف العربية فقط', trigger: 'blur' }
            ]
        },


        

        EnglishOnly() {
            return [
                { required: true, message: 'الرجاء تعبئة البيانات', trigger: 'blur' },
                { min: 3, max: 150, message: 'يجب ان يكون الطول من 3 الي 150', trigger: 'blur' },
                { required: true, pattern: /^[a-zA-Z]+$/, message: 'الرجاء إدخال حروف إنجليزية فقط', trigger: 'blur' }
            ]
        },

        EnglishOnlyNotRequired() {
            return [
                { required: false, message: 'الرجاء تعبئة البيانات', trigger: 'blur' },
                { min: 3, max: 150, message: 'يجب ان يكون الطول من 3 الي 150', trigger: 'blur' },
                { required: false, pattern: /^[a-zA-Z]+$/, message: 'الرجاء إدخال حروف إنجليزية فقط', trigger: 'blur' }
            ]
        },

        DateOnly() {
            return [
                { required: false, message: 'الرجاء إدخال التاريخ', trigger: 'blur' },
                { required: false, pattern: /^(0?[1-9]|[12][0-9]|3[01])[\\/\\-](0?[1-9]|1[012])[\\/\\-]\d{4}$/, message: 'الرجاء إدخال التاريخ بصورة صحيحة', trigger: 'blur' }
            ]
        },

        Required() {
            return [
                { required: true, message: 'الرجاء تعبئة البيانات', trigger: 'blur' },
            ]
        },

        NumberOnlyRequired() {
            return [
                { required: true, message: 'الرجاء تعبئة البيانات', trigger: 'blur' },
                { required: true, pattern: /^[0-9]*$/, message: 'الرجاء إدخال ارقام فقط', trigger: 'blur' }
            ]
        },

        NumberOnlyNotRequired() {
            return [
                { required: false, message: 'الرجاء تعبئة البيانات', trigger: 'blur' },
                { required: false, pattern: /^[0-9]*$/, message: 'الرجاء إدخال ارقام فقط', trigger: 'blur' }
            ]
        },

        NumberOnly() {
            return [
                { required: true, message: 'الرجاء تعبئة البيانات', trigger: 'blur' },
                { min: 3, max: 150, message: 'يجب ان يكون الطول من 3 الي 150', trigger: 'blur' },
                { required: true, pattern: /^[0-9]*$/, message: 'الرجاء إدخال ارقام فقط', trigger: 'blur' }
            ]
        },

        EmailOnly() {
            return [
                { required: true, message: 'الرجاء تعبئة البيانات', trigger: 'blur' },
                { min: 3, max: 150, message: 'يجب ان يكون الطول من 3 الي 150', trigger: 'blur' },
                { required: true, pattern: /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/, message: 'الرجاء إدخال البريد الإلكتروني بطريقة صحيحة', trigger: 'blur' }
            ]
        },

        EmailNotRequierdOnly() {
            return [
                { required: false, message: 'الرجاء تعبئة البيانات', trigger: 'blur' },
                { min: 3, max: 150, message: 'يجب ان يكون الطول من 3 الي 150', trigger: 'blur' },
                { required: false, pattern: /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/, message: 'الرجاء إدخال البريد الإلكتروني بطريقة صحيحة', trigger: 'blur' }
            ]
        },


        NID() {
            return [
                { required: true, message: 'الرجاء ادخال  الرقم الوطني', trigger: 'blur' },
                { min: 12, max: 12, message: "يجب ان يكون طول الرقم الوطني 12 رقم ", trigger: "blur", },
                { required: true, pattern: /^[0-9]*$/, message: 'الرجاء إدخال ارقام فقط', trigger: 'blur' },
            ]
        },
      




        //********************************
        //Gender() {
        //    return [
        //        {
        //            id: 1,
        //            name: "ذكر"
        //        },
        //        {
        //            id: 2,
        //            name: 'أنثي'
        //        }
        //    ];
        //}





    };
})();