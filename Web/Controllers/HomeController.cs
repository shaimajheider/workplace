﻿using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Vue.Models;
using Web.Services;

namespace Vue.Controllers
{
    [Produces("application/json")]
    [Route("Api/Admin/Home")]
    public class HomeController : Controller
    {
        private readonly LotiContext db;
        private IConfiguration _configuration;
        Validation valid;
        Helper help;
        public HomeController(LotiContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            valid = new Validation();
            help = new Helper(configuration);
            this.db = context;
        }


    }
}
