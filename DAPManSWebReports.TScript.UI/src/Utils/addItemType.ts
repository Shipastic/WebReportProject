import { FolderDetail } from "../Models/FolderDetail";

export const addItemType = (childItems: FolderDetail) => {
    const foldersWithType = childItems.childFolders.map((folder: any)  => ({ ...folder, type: 'folder' }));
    const dataViewsWithType = childItems.dataviews.map((dataView: any) => ({ ...dataView, type: 'dataview' }));
    return [...foldersWithType, ...dataViewsWithType];
};
export default addItemType;