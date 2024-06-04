import { DataViewDTO } from '../Models/DataViewDTO';
import { FolderDTO } from '../Models/FolderDTO';
export interface FolderDetail {
    id           : number;
    name         : string;
    childFolders : FolderDTO[];
    dataviews    : DataViewDTO[];
}