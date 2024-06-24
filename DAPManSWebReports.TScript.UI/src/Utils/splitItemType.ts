import { DataViewDTO } from "../Models/DataViewDTO";
import { FolderDTO } from "../Models/FolderDTO";
import { FolderDetail } from "../Models/FolderDetail";

export const splitItemType = (combinedArray: Array<FolderDTO | DataViewDTO & { type: string }>): FolderDetail => {

    const childFolders: FolderDTO[] = combinedArray
        .filter(item => item.type === 'folder')
        // @tts-expect-error
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        .map(({ type, ...folder }) => folder as FolderDTO);
    const dataviews: DataViewDTO[] = combinedArray
        .filter(item => item.type === 'dataview')
        // @tts-expect-error
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        .map(({ type, ...dataview }) => dataview as DataViewDTO);

    return {
        id: 0, 
        name: '', 
        childFolders,
        dataviews,
        totalCount:0,
        offset:0,
        pageSize:0
    };
};
export default splitItemType;
