using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRO_ReceiptsInvMgr.BLL;
using PRO_ReceiptsInvMgr.Domain.IBLL;
using PRO_ReceiptsInvMgr.Model;
using PRO_ReceiptsInvMgr.Model.Tables;

namespace PRO_ReceiptsInvMgr.Application
{
    public class BaseService : IDisposable
    {
        private readonly Lazy<BaseService<DbVersion>> _dbVersionService = new Lazy<BaseService<DbVersion>>();
        private readonly Lazy<BaseService<TConfig>> _configService = new Lazy<BaseService<TConfig>>();
        private readonly Lazy<BaseService<SoftwareVersion>> _softwareVersionService = new Lazy<BaseService<SoftwareVersion>>();
        private readonly Lazy<BaseService<TFpcy>> _fpcyService = new Lazy<BaseService<TFpcy>>();
        private readonly Lazy<BaseService<TDqdm>> _dqdmService = new Lazy<BaseService<TDqdm>>();
       
        protected IBaseService<TConfig> UserService
        {
            get { return _configService.Value; }
        }

        protected IBaseService<DbVersion> DBVersionService
        {
            get { return _dbVersionService.Value; }
        }

        protected IBaseService<SoftwareVersion> SoftwareVersionService
        {
            get { return _softwareVersionService.Value; }
        }
      
        protected IBaseService<TFpcy> FpcyService
        {
            get { return _fpcyService.Value; }
        }
        protected IBaseService<TDqdm> DqdmService
        {
            get { return _dqdmService.Value; }
        }
       
       
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (_configService.IsValueCreated)
            {
                _configService.Value.Dispose();
            }
            if (_dbVersionService.IsValueCreated)
            {
                _dbVersionService.Value.Dispose();
            }
            if (_softwareVersionService.IsValueCreated)
            {
                _softwareVersionService.Value.Dispose();
            }
            if (_fpcyService.IsValueCreated)
            {
                _fpcyService.Value.Dispose();
            }
            if (_dqdmService.IsValueCreated)
            {
                _dqdmService.Value.Dispose();
            }
        }


    }
}
