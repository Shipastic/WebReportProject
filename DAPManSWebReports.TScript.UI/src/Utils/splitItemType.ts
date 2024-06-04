import { DataViewDTO } from "../Models/DataViewDTO";
import { FolderDTO } from "../Models/FolderDTO";
import { FolderDetail } from "../Models/FolderDetail";

export const splitItemType = (combinedArray: Array<FolderDTO | DataViewDTO & { type: string }>): FolderDetail => {
    const childFolders: FolderDTO[] = combinedArray
        .filter(item => item.type === 'folder')
        .map(({ type, ...folder }) => folder as FolderDTO);

    const dataviews: DataViewDTO[] = combinedArray
        .filter(item => item.type === 'dataview')
        .map(({ type, ...dataview }) => dataview as DataViewDTO);

    return {
        id: 0, 
        name: '', 
        childFolders,
        dataviews
    };
};
export default splitItemType;
