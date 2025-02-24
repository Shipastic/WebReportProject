﻿namespace DAPManSWebReports.Domain.FolderService
{
    public interface IMenuService<T1, T2, T3> where T1 : class
                                            where T2 : class
                                            where T3 : class
    {
        // Task<T1> GetListDtosWithParams(IEnumerable<T2> folderDtoList, IEnumerable<T3> dataViewDtoList, int limit, int offset);
        Task<T1> GetListDtos(IEnumerable<T2> folderDtoList, IEnumerable<T3> dataViewDtoList);
    }
}
