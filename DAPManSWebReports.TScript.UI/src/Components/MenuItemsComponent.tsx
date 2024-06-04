//import React,{ useEffect, useState } from 'react';
//import '../App.css';
//import "rsuite/dist/rsuite.css";
//import ChildMenuItemsComponent from './ChildMenuItemsComponent';
////import QueryViewComponent from './QueryViewComponent';
//import { MenuItemDTO } from '../Models/MenuItemDTO';

//import 'rsuite/dist/rsuite-no-reset.min.css';
//import { Dropdown } from 'rsuite';

//interface Props {
//    onSelectDataView: (dataviewid: number) => void; 
//}

//const MenuItemComponent: React.FC<Props> = ({ onSelectDataView }) => {
//    const [items, setItems] = useState<MenuItemDTO>();
//   // const [selectedDataViewId, setSelectedDataViewId] = useState<number | null>(null);

//    useEffect(() => {
//        fetch('https://localhost:7263/api/menu/parents')
//            .then(response => {
//                if (response.ok) {
//                    return response.json();
//                }
//                throw new Error('Network response was not ok.');
//            })
//            .then(data => setItems(data))
//            .catch(error => console.error("There was an error fetching the reports: ", error));
//    }, []);

  
//    return (
//        <>           
//            <Dropdown.Menu title="Catalogs">
//                {items?.childFolders.map((childFolder) => (
//                    <Dropdown.Menu key={childFolder.id} title={childFolder.name} >
//                        <ChildMenuItemsComponent parentid={childFolder.id} onDataViewClick={onSelectDataView} />
//                    </Dropdown.Menu>
//                ))}
//            </Dropdown.Menu>
//            <Dropdown.Menu title="Reports">
//                {items?.dataviews.map((dataview) => (
//                    <Dropdown.Item key={dataview.id} onClick={() => onSelectDataView(dataview.id)}>
//                        {dataview.name}
//                    </Dropdown.Item>
//            ))}
//                </Dropdown.Menu>           
//        </>
//    );
//}
//export default MenuItemComponent;