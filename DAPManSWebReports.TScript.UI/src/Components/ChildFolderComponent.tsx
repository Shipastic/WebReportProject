import React, { useEffect, useState } from 'react';
import { Table, Column, HeaderCell, Cell } from 'rsuite-table';
import { FolderDetail       } from '../Models/FolderDetail';
import { defaultColumns     } from '../constants/ColumnsBrowseTable';
import { addItemType        } from '../Utils/addItemType';
import { splitItemType      } from '../Utils/splitItemType';
import { getSortedData } from '../Utils/sortUtils';
import QueryViewComponent from '../Components/QueryViewComponent';
import '../App.css';
import '../App.scss';
interface Props
{
    parentid: number | null;
    path: string;
    updateBreadcrumbs: (breadcrumbs: string[]) => void;
    breadcrumbs: string[];
}

const CustomHeaderCell = ({ ...props }) => (
    <HeaderCell {...props} style={{ backgroundColor: '#8B4513', color: 'white', fontWeight: 'bold', padding: '4px 0 4px 10', position: 'sticky', top: 0, zIndex: 10}} />
  );
  

const ChildFolderComponent: React.FC<Props> = ({ parentid, path, updateBreadcrumbs, breadcrumbs }) =>
{
    const [childItems, setItems] = useState<FolderDetail | null>(null);
    const [sortType, setSortType] = useState<'asc' | 'desc' | undefined>();
    const [sortColumn, setSortColumn] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [activeFolderId, setActiveFolderId] = useState<number | null>(null);
    const [activeDataviewId, setActiveDataviewId] = useState<number | null>(null);

    const [error,          setError         ] = useState<string | null>(null);

    const [needsDateParams, setNeedsDateParams] = useState(false);

    let pathArray     : string[] = [];
    let newBreadcrumbs: any[] = [];

    const fetchData = async () =>
    {
        setLoading(true);
        try
        {
            const response = await fetch(`https://localhost:7263/api/menu/childrens/${parentid}`);
            if (response.ok)
            {
                const data = await response.json();
                setItems(data);            
            }
            else
            {
                console.error('Network response was not ok.');
            }
        }
        catch (error)
        {
            console.error('There was an error!', error);
        }
        finally
        {
            setLoading(false);
        }
    };

    useEffect(() =>
    {
        fetchData();
        
    }, [parentid]);


    const updateUI = () =>
    {     
        fetchData();
    };
    const handleSortColumn = (sortColumn: string, sortType: 'asc' | 'desc') =>
    {
        setLoading(true);
        setTimeout(() =>
        {
            setLoading(false);
            setSortColumn(sortColumn);
            setSortType(sortType);
        }, 500);
    };

    const handleCellClick = async ({ rowData, dataKey }) =>
    {
        const rowDataId = rowData.id;
        const newPath = path ? `${path}/${rowData.name}` : rowData.name;
        newBreadcrumbs = [...breadcrumbs, rowData.name];
        pathArray.push(path, newPath);
        updateBreadcrumbs(newBreadcrumbs);
        try
        {
            if (rowData.type === 'folder')
            {
                const hasChildren = await hasChildElements(rowDataId);
                if (hasChildren)
                {
                    setSampleParentid(rowDataId, rowData.id);
                    if (dataKey === 'name')
                    {
                        const tempParentId = findFolderByName(rowData.name, combinedArray);
                        const res: FolderDetail = splitItemType(combinedArray);
                        setItems(res);
                        parentid = tempParentId;                       
                        setActiveFolderId(tempParentId); 
                        updateUI();  
                    }                                    
                }
                else
                {
                    console.log(`Объект с id ${rowDataId} не имеет дочерних элементов.`);
                }
            }
            if (rowData.type === 'dataview')
            {
                if (dataKey === 'name')
                {                  
                    const tempDataViewId = findDataviewByName(rowData.name, combinedArray);
                    checkColumnPresence(tempDataViewId);
                    setActiveDataviewId(tempDataViewId);                                
                }
            }
        }
        catch (error)
        {
           console.error("Ошибка при проверке дочерних элементов:", error);
        }
    };

    const setSampleParentid = (id: number, newParentid: number) =>
    {
        const index = combinedArray.findIndex(item => item.id === id);
        if (index !== -1)
        {
        combinedArray[index].parentid = newParentid;
        }
        else
        {
        console.log(`Объект с id ${id} не найден.`);
         }
    };
    
    const findFolderByName = (name:string, combinedArray: any[]) =>
    {
        const folder = combinedArray
                                .find(item => item.type === 'folder' && item.name === name);
        return folder ? folder.id : null;
    };

    const findDataviewByName = (name: string, combinedArray: any[]) =>
    {
        const dataview = combinedArray
                                .find(item => item.type === 'dataview' && item.name === name);
        return dataview ? dataview.id : null;
    };

    const hasChildElements = async (parentid:number) =>
    {
        const response = await fetch(`https://localhost:7263/api/menu/childrens/${parentid}`);
        if (response.ok)
        {
            const data = await response.json();
            return data.childFolders.length > 0 || data.dataviews.length > 0;
        }
        return false;
    };

    const checkColumnPresence = async (dataviewid: number) => {
        try {
            const response = await fetch(`https://localhost:7263/api/dataview/${dataviewid}`);
            const result = await response.json();
            if (result.startDateField != null && result.endDateField != null)
            setNeedsDateParams(true);
        } catch (error: any) {
            setError(error.message);
        }
    };

    if (loading) {
        return <div>Loading...</div>;
    };

    if (activeDataviewId) {
        return <QueryViewComponent
                        dataviewid={activeDataviewId}
                        path={pathArray[1]}
                        updateBreadcrumbs={updateBreadcrumbs}
                        breadcrumbs={newBreadcrumbs}
                        needsDateParams={needsDateParams}
                />;       
    };   

    if (childItems === null) {
        return <div>No data available</div>;
    };

    const combinedArray = childItems ? addItemType(childItems) : [];

    const data = getSortedData(combinedArray, sortColumn, sortType);   

    const tableStyle = {
        maxHeight: '770px', //высота
        maxWidth: '1850px',
        overflowY: 'auto', // вертикальная прокрутка
      };

    return (
        <div style={{ margin: '10px 10px 0px 15px' }} className="query-view-container">
            <Table
                    data={data}
                    bordered
                    cellBordered
                    sortColumn={sortColumn}
                    sortType={sortType}
                    onSortColumn={handleSortColumn}
                    loading={loading}
                    autoHeight                  
                    autoFocus={true}
                    autoCorrect="true"
                    style={tableStyle}
                    
                >
                {defaultColumns.map(column =>
                (
                    <Column key={column.key} sortable resizable fullText flexGrow={1}>
                        <CustomHeaderCell >{column.label}</CustomHeaderCell >
                        <Cell
                            className={column.key === 'name' ? "clickable-cell" : ''}
                            dataKey={column.key}
                            style={{ padding:'5px 0px 0px 5px' }}
                        >
                            {rowData => {
                                if (column.key === 'download' && rowData.type === 'dataview')
                                {
                                    return (
                                        <a
                                            onClick={() => console.log('Download clicked')} // Добавить логику здесь
                                            
                                            style={{ padding: '5px 0px 0px 0px' , textAlign:'right'}}
                                            href='#' >
                                                Скачать
                                        </a>                                       
                                );}
                                return (
                                    <div
                                        className={column.key === 'name' ? "clickable-cell" : ''}
                                        onClick={() => handleCellClick({ rowData, dataKey: column.key })}
                                    >
                                        
                                        {rowData[column.key]}
                                    </div>
                                );
                            }}
                        </Cell>
                    </Column>
                )
                )}                               
            </Table>           
        </div>
    );
};
export default ChildFolderComponent;

