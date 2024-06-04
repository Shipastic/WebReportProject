import { useEffect, useState } from 'react';
import { DataViewDTO } from '../Models/DataViewDTO';
import QueryViewComponent from './QueryViewComponent';
import { Dropdown } from 'rsuite';


interface Props {
    parentid: number | null;
}

const ChildDataViewComponent: React.FC<Props> = ({ parentid }) => {
    const [childDataViews, setViews]                  = useState<DataViewDTO[]>([]);
    //const [activeChildViewId, setActiveChildViewId]   = useState<number | null>(null);
    const [selectedDataViewId, setSelectedDataViewId] = useState<number | null>(null);

    useEffect(() => {
        if (parentid === null) return;
        fetch(`https://localhost:7263/api/dataviews/childrens/${parentid}`)
            .then(response => {
                if (response.ok) {
                    return response.json();
                }
                throw new Error('Network response was not ok.');
            })
            .then((data) => setViews(data))
            .catch((error) => console.error('There was an error!', error));
    }, [parentid]);

    
    if (parentid === null) {
        return null;
    }

    return (
        <>     
       <ul>
                {childDataViews.map(childDataView => (
                    <Dropdown.Item key={childDataView.id} onClick={() => setSelectedDataViewId(childDataView.id)}>    
                    {childDataView.name}
                    <ChildDataViewComponent parentid={selectedDataViewId} />
                    </Dropdown.Item>

                ))}           
        </ul>       
            {selectedDataViewId !== null && (
                <QueryViewComponent dataviewid={selectedDataViewId} />
            )}
        </>
    );
};
export default ChildDataViewComponent;