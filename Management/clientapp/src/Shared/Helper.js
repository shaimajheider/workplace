//import moment from 'moment';
import Swal from "sweetalert2";
//import blockUI from "./BlockUIService.js";
//import DataService from "./DataService.js";

export default (function () {
    return {
        //******************************************** Moment Info ********************************************

        //GetCities() {
        //    var res;
        //    blockUI.Start();
        //    DataService.GetCitiesName()
        //        .then((response) => {
        //            blockUI.Stop();
        //            res = response.data.info;
        //        })
        //        .catch(() => {
        //            blockUI.Stop();
        //        });
        //    return res;
        //},

        //******************************************** Moment Info ********************************************
        //moment: function (date) {
        //    if (date === null) {
        //        return "فارغ";
        //    }
        //    // return moment(date).format('MMMM Do YYYY, h:mm:ss a');
        //    return moment(date).format('MMMM Do YYYY');
        //}

        //******************************************** User Info ********************************************

        //CheckLoginStatus() {
        //    try {
        //        let loginDetails = JSON.parse(
        //            localStorage.getItem("currentUser-client")
        //        );
        //        if (this.loginDetails == null) {
        //            window.location.href = "/Login";
        //        }
        //        return loginDetails;
        //    } catch (error) {
        //        window.location.href = "/Login";
        //    }
        //},

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
                    this.showSwal("warning");
                }
            });
            return res;
        },

        //******************************************** Message Info ********************************************
        showSwal(type) {
            if (type === "warning") {
                Swal.fire({
                    icon: "warning",
                    title: "عملية غير ناجحة",
                    html: "الرجاء التحقق من ادخال جميع البيانات",
                    showCancelButton: false,
                }).then(() => { });
            }
        },

        showWorning(text) {
            Swal.fire({
                icon: "warning",
                title: "عملية غير ناجحة",
                html: text,
                showCancelButton: false,
            }).then(() => { });
        },

        ShowMessage(icon, title, html) {
            Swal.fire({
                icon: icon,
                title: title,
                html: html,
                showCancelButton: false,
            }).then(() => { });
        },

        //******************************************** Object Info ********************************************

        DynamicArabicEnterRequired(property) {
            return [
                { required: true, message: 'الرجاء التأكد من إدخال ' + property, trigger: 'blur' },
                { min: 3, max: 250, message: 'يجب ان يكون الطول من 3 الي 250', trigger: 'blur' },
                { required: true, pattern: /[\u0600-\u06FF]/, message: 'الرجاء إدخال حروف العربية فقط', trigger: 'blur' }
            ]
        },

        DynamicArabicSelectRequired(property) {
            return [
                { required: true, message: 'الرجاء التأكد من إختيار' + property, trigger: 'blur' },
                { min: 3, max: 250, message: 'يجب ان يكون الطول من 3 الي 250', trigger: 'blur' },
                { required: true, pattern: /[\u0600-\u06FF]/, message: 'الرجاء إدخال حروف العربية فقط', trigger: 'blur' }
            ]
        },

        DynamicEnglishEnterRequired(property) {
            return [
                { required: true, message: 'الرجاء التأكد من إدخال ' + property, trigger: 'blur' },
                { min: 3, max: 250, message: 'يجب ان يكون الطول من 3 الي 250', trigger: 'blur' },
                { required: true, pattern: /^[a-zA-Z]+$/, message: 'الرجاء إدخال حروف إنجليزية فقط', trigger: 'blur' }
            ]
        },

        DynamicEnglishSelectRequired(property) {
            return [
                { required: true, message: 'الرجاء التأكد من إختيار' + property, trigger: 'blur' },
                { min: 3, max: 250, message: 'يجب ان يكون الطول من 3 الي 250', trigger: 'blur' },
                { required: true, pattern: /^[a-zA-Z]+$/, message: 'الرجاء إدخال حروف إنجليزية فقط', trigger: 'blur' }
            ]
        },

        ArabicOnly() {
            return [
                { required: true, message: 'الرجاء تعبئة البيانات', trigger: 'blur' },
                { min: 3, max: 250, message: 'يجب ان يكون الطول من 3 الي 250', trigger: 'blur' },
                { required: true, pattern: /[\u0600-\u06FF]/, message: 'الرجاء إدخال حروف العربية فقط', trigger: 'blur' }
            ]
        },


        EnglishOnly() {
            return [
                { required: true, message: "الرجاء تعبئة البيانات", trigger: "blur" },
                {
                    min: 3,
                    max: 250,
                    message: "يجب ان يكون الطول من 3 الي 250",
                    trigger: "blur",
                },
                {
                    required: true,
                    pattern: /^[a-zA-Z]+$/,
                    message: "الرجاء إدخال حروف إنجليزية فقط",
                    trigger: "blur",
                },
            ];
        },

        Phone() {
            return [
                {
                    required: true,
                    message: "الرجاء إدخال رقم الهاتف",
                    trigger: "blur",
                },
                {
                    min: 9,
                    max: 13,
                    message: "يجب ان يكون طول رقم الهاتف 9 ارقام علي الاقل",
                    trigger: "blur",
                },
                { required: true, pattern: /^[0-9]*$/, message: 'الرجاء إدخال ارقام فقط', trigger: 'blur' }
            ];
        },

        EnglishOnlyNotRequired() {
            return [
                { required: false, message: "الرجاء تعبئة البيانات", trigger: "blur" },
                {
                    min: 3,
                    max: 250,
                    message: "يجب ان يكون الطول من 3 الي 250",
                    trigger: "blur",
                },
                {
                    required: false,
                    pattern: /^[a-zA-Z]+$/,
                    message: "الرجاء إدخال حروف إنجليزية فقط",
                    trigger: "blur",
                },
            ];
        },

        DateOnly() {
            return [
                { required: false, message: "الرجاء إدخال التاريخ", trigger: "blur" },
                {
                    required: false,
                    pattern: /^(0?[1-9]|[12][0-9]|3[01])[\\/\\-](0?[1-9]|1[012])[\\/\\-]\d{4}$/,
                    message: "الرجاء إدخال التاريخ بصورة صحيحة",
                    trigger: "blur",
                },
            ];
        },

        Required() {
            return [
                { required: true, message: "الرجاء تعبئة البيانات", trigger: "blur" },
            ];
        },


        RequiredInput(input) {
            return [
                { required: true, message: "الرجاء إدخال بيانات " + " "+input, trigger: "blur" },
            ];
        },

        NumberOnlyRequired() {
            return [
                { required: true, message: "الرجاء تعبئة البيانات", trigger: "blur" },
                {
                    required: true,
                    pattern: /^[0-9]*$/,
                    message: "الرجاء إدخال ارقام فقط",
                    trigger: "blur",
                },
            ];
        },

        NumberOnlyNotRequired() {
            return [
                { required: false, message: "الرجاء تعبئة البيانات", trigger: "blur" },
                {
                    required: false,
                    pattern: /^[0-9]*$/,
                    message: "الرجاء إدخال ارقام فقط",
                    trigger: "blur",
                },
            ];
        },

        NumberOnly() {
            return [
                { required: true, message: "الرجاء تعبئة البيانات", trigger: "blur" },
                {
                    min: 0,
                    max: 100,
                    message: "يجب ان يكون الطول من 3 الي 100",
                    trigger: "blur",
                },
                {
                    required: true,
                    pattern: /^[0-9]*$/,
                    message: "الرجاء إدخال ارقام فقط",
                    trigger: "blur",
                },
            ];
        },

        LoginName() {
            return [
                { required: true, message: 'الرجاء إدخال اسم الدخول', trigger: 'blur' },
                { required: true, pattern: /^[A-Za-z]{0,40}$/, message: 'الرجاء إدخال اسم الدخول بطريقه صحيحه', trigger: 'blur' }
            ];
        },

        Email() {
            return [
                { required: true, message: 'الرجاء إدخال البريد الإلكتروني', trigger: 'blur' },
                { required: true, pattern: /\S+@\S+\.\S+/, message: 'الرجاء إدخال البريد الإلكتروني بطريقه صحيحه', trigger: 'blur' }
            ];
        },

        EmailOnly() {
            return [
                { required: true, message: "الرجاء تعبئة البيانات", trigger: "blur" },
                {
                    min: 3,
                    max: 250,
                    message: "يجب ان يكون الطول من 3 الي 250",
                    trigger: "blur",
                },
                {
                    required: true,
                    pattern: /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/,
                    message: "الرجاء إدخال البريد الإلكتروني بطريقة صحيحة",
                    trigger: "blur",
                },
            ];
        },

        EmailNotRequierdOnly() {
            return [
                { required: false, message: "الرجاء تعبئة البيانات", trigger: "blur" },
                {
                    min: 3,
                    max: 250,
                    message: "يجب ان يكون الطول من 3 الي 250",
                    trigger: "blur",
                },
                {
                    required: false,
                    pattern: /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/,
                    message: "الرجاء إدخال البريد الإلكتروني بطريقة صحيحة",
                    trigger: "blur",
                },
            ];
        },

        NID() {
            return [
                { required: true, message: 'الرجاء ادخال  الرقم الوطني', trigger: 'blur' },
                { min: 12, max: 12, message: "يجب ان يكون طول الرقم الوطني 12 رقم ", trigger: "blur", },
                { required: true, pattern: /^[0-9]*$/, message: 'الرجاء إدخال ارقام فقط', trigger: 'blur' },
            ]
        },

    };
})();
