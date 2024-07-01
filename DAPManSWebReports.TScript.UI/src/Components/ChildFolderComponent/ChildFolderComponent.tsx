import React, { useEffect, useState      } from 'react';
import { Table, Column, HeaderCell, Cell } from 'rsuite-table';
import { FolderDetail                    } from '../../Models/FolderDetail';
import { defaultColumns                  } from '../../constants/ColumnsBrowseTable';
import { addItemType                     } from '../../Utils/addItemType';
import { splitItemType                   } from '../../Utils/splitItemType';
import { getSortedData                   } from '../../Utils/sortUtils';
import { DataViewDTO                     } from '../../Models/DataViewDTO';
import QueryViewComponent                  from '../QueryViewComponent/QueryViewComponent';
import AddEditReportModal                  from '../../Modals/AddEditModal/AddEditModalWindow';
import config                              from '../../Utils/config';
import tokenService                        from '../../Services/tokenService';
import './ChildFolder.css';

interface Props
{
    parentid: number | null;
    path: string;
    updateBreadcrumbs: (breadcrumbs: string[]) => void;
    breadcrumbs: string[];
}

const CustomHeaderCell = ({ ...props }) => 
    (
        <HeaderCell children={undefined} {...props} className='cf-header-cell' />
    );
  
const ChildFolderComponent: React.FC<Props> = ({ parentid, path, updateBreadcrumbs, breadcrumbs }) =>
{
    const [childItems, setItems                 ] = useState<FolderDetail | null>(null);
    const [sortType, setSortType                ] = useState<'asc' | 'desc' | undefined>();
    const [sortColumn, setSortColumn            ] = useState<string | null>(null);
    const [loading, setLoading                  ] = useState<boolean>(false);
    const [activeDataviewId, setActiveDataviewId] = useState<number | null>(null);
    const [showModal, setShowModal              ] = useState(false);
    const [currentReport, setCurrentReport      ] = useState<DataViewDTO | null>(null);

    const pathArray     : string[] = [];
    let newBreadcrumbs: any[] = [];

    const fetchData = async () =>
    {
        setLoading(true);
        const headers = tokenService.getAuthHeaders();
        try
        {
            const response = await fetch(`${config.ApiBaseUrlDev}/menu/childrens/${parentid}`, {headers: headers});
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

    const handleEditClick = async ({rowData}) => 
        {
            const headers = tokenService.getAuthHeaders();
            try 
            {
                const dataviewid = rowData.id;                           
                const response = await fetch(`${config.ApiBaseUrlDev}/dataview/${dataviewid}`, {headers:headers});            
              if (response.ok) 
                {
                    console.log('Response Status:', response.status);
                    const data = await response.json();
                    console.log('Response Text:', response);
                    setCurrentReport(data);
                    setShowModal(true);
                } 
                else 
                {
                    console.error('Error fetching report data:', response.statusText);
                }
            } 
            catch (error) 
            {
              console.error('Error fetching report data:', error);
            }
        };

    const handleSave = async (report: DataViewDTO) => 
        {
          setItems(prev => {
              if (prev) {
                  return {
                      ...prev,
                      items: prev.dataviews.map(item => item.id === report.id ? report : item)
                  };
              }
              return prev;
          });
          setShowModal(false);
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
            const response = await fetch(`${config.ApiBaseUrlDev}/menu/childrens/${parentid}`);
            if (response.ok)
            {
                const data = await response.json();
                return data.childFolders.length > 0 || data.dataviews.length > 0;
            }
            return false;
        };

    if (loading) 
        {
            return <div>Loading...</div>;
        }

    if (activeDataviewId) 
        {
            return <QueryViewComponent
                    dataviewid={activeDataviewId}
                    path={pathArray[1]}
                    updateBreadcrumbs={updateBreadcrumbs}
                    breadcrumbs={newBreadcrumbs}               
                    />;       
        }  

    if (childItems === null) 
        {
            return <div>No data available</div>;
        }

    const combinedArray = childItems ? addItemType(childItems) : [];

    const data = getSortedData(combinedArray, sortColumn, sortType);   

    return (
        <div className="cf-folder-view-container">
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
                 //@ts-ignore
                className='tableStyle'                  
            >
                {defaultColumns.map(column =>
                (
                    <Column key={column.key} sortable resizable fullText flexGrow={1}>
                        <CustomHeaderCell >{column.label}</CustomHeaderCell >
                        <Cell
                            className={column.key === 'name' ? "cf-clickable-cell-table" : 'cf-cell-table'}
                            dataKey={column.key}        
                        >
                            {rowData => {
                                if (column.key === 'edit' && rowData.type === 'dataview')
                                {
                                    return (
                                        <a onClick={() => handleEditClick({rowData})} href='#' >
                                            Редактировать | Удалить
                                        </a>                                       
                                );}
                                return (
                                    <div
                                        className={column.key === 'name' ? "cf-clickable-cell" : 'cf-cell'}
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
            {showModal && currentReport && (
            <AddEditReportModal
                show={showModal}
                report={currentReport}
                onSave={handleSave}
                onClose={() => setShowModal(false)}
            />
            )}           
        </div>
    );
};
export default ChildFolderComponent;

