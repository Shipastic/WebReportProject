import { DataViewDTO } from '../Models/DataViewDTO';
import { FolderDTO } from '../Models/FolderDTO';
export interface FolderDetail {
    id           : number;
    name         : string;
    childFolders : FolderDTO[];
    dataviews    : DataViewDTO[];
    totalCount: number;
    offset: number;
    pageSize: number;
}