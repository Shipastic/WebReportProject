//import React, { useEffect, useState } from 'react';
//import '../App.css';
//import "rsuite/dist/rsuite.css";
//import { MenuItemDTO } from '../Models/MenuItemDTO';
//import { Dropdown } from 'rsuite';

//interface Props {
//    parentid: number | null;
//    depth?: number;
//    onDataViewClick: (dataviewid: number) => void;
//}

//const MAX_DEPTH = 4;

//const ChildMenuItemsComponent: React.FC<Props> = ({ parentid, depth = 0, onDataViewClick }) => {
//    const [childItems, setChilds] = useState<MenuItemDTO | null>(null);

//    useEffect(() => {
//        if (parentid === null || depth > MAX_DEPTH) return;

//        const fetchData = async () => {
//        try {
//            const response = await fetch(`https://localhost:7263/api/menu/childrens/${parentid}`);
//            if (!response.ok) {
//                throw new Error('Network response was not ok.');
//            }
//            const data = await response.json();
//            setChilds(data);
//        } catch (error) {
//            console.error('There was an error!', error);
//        }
//        };

//        fetchData();
//    }, [parentid, depth]);

//    if (parentid === null) {
//        return <div>Loading...</div>;
//    }

//    return (
//       <>
//            {childItems?.childFolders.map((childFolder) => (
//                <Dropdown.Menu key={childFolder.id} title={childFolder.name}>
//                    <ChildMenuItemsComponent parentid={childFolder.id} depth={depth + 1} onDataViewClick={onDataViewClick} />
//                </Dropdown.Menu>
//            ))}

//            {childItems?.dataviews.map((dataview) => (
//                <Dropdown.Item key={dataview.id} onClick={() => onDataViewClick(dataview.id)}>
//                    {dataview.name}
//                </Dropdown.Item>
//            ))}          
//        </>
//    );
//};
//export default ChildMenuItemsComponent;