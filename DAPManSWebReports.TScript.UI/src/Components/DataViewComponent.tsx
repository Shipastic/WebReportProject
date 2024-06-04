//import { useEffect, useState } from 'react';
//import ChildFolderComponent from './ChildFolderComponent';
//import { DataViewDTO } from '../Models/DataViewDTO';

//const DataViewComponent: React.FC = () =>
//{
//    const [folders, setFolders] = useState<DataViewDTO[]>([]);
//    const [activeFolderId, setActiveFolderId] = useState<number | null>(null);

//    const handleFolderClick = (id: number) => {
//        console.log(`Setting parentId to ${id}`);
//        setActiveFolderId(id);
//    };

//    useEffect(() => {
//        fetch('https://localhost:7263/api/dataviews/parents')
//            .then(response => {
//                if (response.ok) {
//                    return response.json();
//                }
//                throw new Error('Network response was not ok.');
//            })
//            .then(data => setFolders(data))
//            .catch(error => console.error("There was an error fetching the reports: ", error));
//    }, []);

//    return (
//        <ul>
//            {folders.map((folder) => (
//                <li key={folder.id} onClick={() => handleFolderClick(folder.id)}>
//                    {folder.name}
//                    {activeFolderId === folder.id && <ChildFolderComponent parentid={folder.id} />}
//                </li>
//            ))}
//        </ul>
//    );
//}
//export default DataViewComponent;
