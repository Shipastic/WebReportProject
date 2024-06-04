import React, { useEffect, useState } from 'react';
import ChildFolderComponent from './ChildFolderComponent';
import 'rsuite/dist/rsuite.min.css';
import { Dropdown} from 'rsuite';
import { FolderDetail } from '../Models/FolderDetail';
import QueryViewComponent from './QueryViewComponent';

const FolderComponent: React.FC = () => 
{
    const [items, setItems] = useState<FolderDetail | null>(null);
    const [activeFolderId, setActiveFolderId] = useState<number | null>(null);
    const [selectedDataViewId, setSelectedDataViewId] = useState<number | null>(null);

        useEffect(() =>
        {
            fetch('https://localhost:7263/api/menu/parents')
            .then(response => {
                if (response.ok) {
                    return response.json();
                }
                throw new Error('Network response was not ok.');
            })
            .then(data => setItems(data))
            .catch(error => console.error("There was an error fetching the reports: ", error));
        }, []);

    const handleItemClick = (id: number) =>
    {
        setActiveFolderId(id);
    };
    const handleDataViewClick = (dataviewid: number) =>
    {
        setSelectedDataViewId(dataviewid);
    };

    return (
        <>
            {items?.childFolders.map(childFolder => (
                <Dropdown.Item
                    key={childFolder.id}
                    eventKey={childFolder.id}
                    onSelect={() => handleItemClick(childFolder.id)}
                > 
                    {childFolder.name}
                </Dropdown.Item>
            ))}
          <hr/>
            {items?.dataviews.map(dataview => (
                <Dropdown.Item
                    key={dataview.id}
                    onClick={() => handleDataViewClick(dataview.id)}
                >
                    {dataview.name}
               </Dropdown.Item>
            ))}
            {activeFolderId !== null && (
                <ChildFolderComponent
                    parentid={activeFolderId} />
            )}
            {/*             
            {selectedDataViewId !== null && (
                <div className="query-view-container">
                    <QueryViewComponent dataviewid={selectedDataViewId} />
                </div>
            )}
           */}
        </>
            );
}
export default FolderComponent;
