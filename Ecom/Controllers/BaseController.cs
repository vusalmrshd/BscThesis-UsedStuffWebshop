using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Core.Interface;
using Microsoft.Extensions.Configuration;

namespace Ecom.Controllers.APi
{
    public class BaseController : ControllerBase
    {
        private IAuthService _authService;

        public IAuthService _AuthService
        {
            get { return _authService ?? HttpContext.RequestServices.GetService<IAuthService>(); }
            set { _authService = value; }
        }

        private ICommonService _commonService;

        public ICommonService _CommonService
        {
            get { return _commonService ?? HttpContext.RequestServices.GetService<ICommonService>(); }
            set { _commonService = value; }
        }

        private IConfiguration _config;

        public IConfiguration _Config
        {
            get { return _config ?? HttpContext.RequestServices.GetService<IConfiguration>(); }
            set { _config = value; }
        }

        private IOrderService _order;

        public IOrderService _OrderService
        {
            get { return _order ?? HttpContext.RequestServices.GetService<IOrderService>(); }
            set { _order = value; }
        }

        private IProductService _product;

        public IProductService _ProductService
        {
            get { return _product ?? HttpContext.RequestServices.GetService<IProductService>(); }
            set { _product = value; }
        }

    }
}
