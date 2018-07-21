using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Script.BLL.Shared
{
    public interface IBLLFunctions<TEntity, TViewModel>
        where TEntity : class
        where TViewModel : class
    {
        List<TEntity> GetData();
        List<TViewModel> GetModels();
        TEntity GetDataById(object id);
        int SetModel(TViewModel model);
        TViewModel GetModelById(object id);
        TViewModel GetDataViewModel(TEntity data);
        List<TViewModel> GetDataViewModel(IEnumerable<TEntity> data);
        TEntity GetDataByViewModel(TViewModel model);
        List<TEntity> GetDataByViewModel(IEnumerable<TViewModel> model);
        int Create(TViewModel model);
        void SetListContext();
        void SetListModel();
    }
}
