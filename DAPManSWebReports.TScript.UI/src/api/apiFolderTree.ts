import config from '../Utils/config';
import { FolderDetail } from '../Models/FolderDetail';

const ApiBaseUrl = config.ApiBaseUrl;

export const fetchChildItems = async (parentid: number | null): Promise<FolderDetail | null> => {

    try {
        const response = await fetch(`${ApiBaseUrl}/menu/childrens/${parentid}`);
        if (response.ok) {
            return response.json();
        }
        throw new Error('Network response was not ok.');
    } catch (error) {
        console.error('There was an error!', error);
        return null;
    }
};