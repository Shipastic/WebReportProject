import React,{ useEffect, useState } from 'react';
import { QueryViewDTO } from '../Models/QueryViewDTO';
import '../App.css';
import '../App.scss';
import { Placeholder,Table, Pagination, TagPicker, Button, Row, Col, Grid, Stack, Message, Loader } from 'rsuite';
import QueryparamsComponent from './QueryParamsComponent'; 
import FilterComponent from './FilterComponent';
import 'rsuite/dist/rsuite.css';
import 'rsuite-table/dist/css/rsuite-table.css' ;
import StepsTutorialComponent from './StepsTutorialComponent';

const { Column, HeaderCell, Cell } = Table;
interface Props {
    dataviewid: number | null;
    path: string;
    updateBreadcrumbs: (breadcrumbs: string[]) => void;
    breadcrumbs: string[];
    needsDateParams: boolean;
}
interface ViewParams {
    startDate: string;
    endDate: string;
    presetDate: string; 
    format: string;
    sortOrder: string;
    sortColumnNumber: number;
    
}
interface PagedResult<T> {
    items: T[];
    totalCount: number;
    offset: number;
    pageSize: number;
}

const CustomHeaderCell = ({ ...props }) => (
    <HeaderCell {...props} style={{ backgroundColor: '#4CAF50', color: 'white', fontWeight: 'bold', padding: '4px 0 2px 0'}} />
  );
  
const QueryViewComponent: React.FC<Props> = ({ dataviewid, path, updateBreadcrumbs, breadcrumbs, needsDateParams }) => {
    const [queryviews,     setData          ] = useState<QueryViewDTO | null>(null);
    const [loading,        setLoading       ] = useState<boolean>(false);
    const [loadingTabe,    setLoadingTable  ] = useState<boolean>(false);
    const [offset,         setPage          ] = useState(1);
    const [limit,          setLimit         ] = useState(10);
    const [totalCount,     setTotalCount    ] = useState(0);
    const [sortColumn,     setSortColumn    ] = useState<string | null>(null);
    const [sortType,       setSortType      ] = useState<'asc' | 'desc' | undefined>();
    const [error,          setError         ] = useState<string | null>(null);
    const [columnKeys,     setColumnKeys    ] = useState<string[]>([]);

    const [viewParams,     setViewParams    ] = useState<ViewParams>({ startDate: '', endDate: '', presetDate: '', format:'EXCEL', sortOrder:'asc', sortColumnNumber:1 });
    const [showTable,      setShowTable     ] = useState<boolean>(false); 

    const [filterValue,    setFilterValue   ] = useState('');
    const [filteredData,   setFilteredData  ] = useState( Array<{ [key: string]: any }>);
    const [selectedColumn, setSelectedColumn] = useState('');

    useEffect(() => {
        if (viewParams.startDate && viewParams.endDate)
        {
            setLoading(true);
            fetchdata(dataviewid, offset, limit, viewParams);
        }
    }, [dataviewid, offset, limit, viewParams]);

    {/*
    const checkColumnPresence = async (dataviewid: number) => {
        try {
            const response = await fetch(`https://localhost:7263/api/dataview/${dataviewid}`);
            const result = await response.json();
            if (result.StartDate != '' && result.EndDate != '')
            setNeedsDateParams(true);
        } catch (error: any) {
            setError(error.message);
        }
    };
*/}

    const fetchdata = async (dataviewid: number | null, offset: number, limit: number, viewParams: ViewParams) => {   
        if (dataviewid === null) return;
        try
        {                             
            const params = new URLSearchParams({
                limit: limit.toString(),
                offset: offset.toString(),
                startDate: viewParams.startDate,
                endDate: viewParams.endDate,
                presetDate: viewParams.presetDate,
                format: viewParams.format,
                sortOrder: viewParams.sortOrder,
                sortColumnNumber: viewParams.sortColumnNumber.toString()
            });
            const response = await fetch(`https://localhost:7263/api/query/${dataviewid}?${params.toString()}`);
           if (!response.ok)
           {
               throw new Error(`Error: ${response.status}`);
           }
           const result = await response.json();
            setData(result);
            setTotalCount(result.totalCount);
            setPage(result.offset);
            setLimit(result.pageSize);
           const headers = result.result.length> 0 ? Object.keys(result.result[0]) : [];
           
            setColumnKeys(headers);
            setShowTable(result.result.length > 0);

            setFilteredData(result.result);
        }
        catch (error: any)
        {
            setError(error.message);
        }
        finally
        {
            setLoading(false);
            setLoadingTable(false);
        }   
    };
    const handlePageChange = (page) => {
        setPage(page);
    };
    const handleParamsChange = (params: ViewParams) => {
        setViewParams(params);
    };

    const getData = (sortColumn: string | null, sortType: 'asc' | 'desc' | undefined) => {
        if (!queryviews || !queryviews.result)
            return [];
        if (sortColumn && sortType) {
            return [...queryviews.result].sort((a, b) => {
                const x = a[sortColumn];
                const y = b[sortColumn];
               
                if (typeof x === 'number' && typeof y === 'number') {
                    return sortType === 'asc' ? x - y : y - x;
                }

                if (typeof x === 'string' && typeof y === 'string') {
                    return sortType === 'asc' ? x.localeCompare(y) : y.localeCompare(x);
                }
                
                console.warn('Unsupported sort type or values', x, y);
                return 0;
            });
        }
        return queryviews.result;
    };


    const data = getData(sortColumn, sortType);   

    const headers = data.length > 0 ? Object.keys(data[0]) : [];

    const handleChangeLimit = (dataKey: number) => {
        // Resetting to first page when limit changes
        setPage((page - 1) * dataKey);  
        setLimit(dataKey);
    };

    const handleSortColumn = (sortColumn: string, sortType: 'asc' | 'desc') => {
        setLoading(true);
        setTimeout(() => {
            setLoading(false);
            setSortColumn(sortColumn);
            setSortType(sortType);
        }, 500);
    };
   
    const handleApplyFilter = () => {
        let filtered =  getData(sortColumn, sortType);
        if (selectedColumn && filterValue) {
            filtered = filtered.filter(item => {
                const value = item[selectedColumn];
                if (typeof value === 'number') {
                    return value.toString() === filterValue;
                }
                return value !== undefined && value.toString().toLowerCase().includes(filterValue.toLowerCase());
            });
        }
        setFilteredData(filtered);
    };
    
      const resetFilter = () => {
        setFilterValue('');
        setSelectedColumn('');
        setFilteredData(queryviews?.result || []);
      };

    return (
        <>
        <Grid fluid>
        {!showTable  ? 
        (
         <>       
            <Row> 
                <Col xs={12}>
                <Stack style={{ textAlign: 'left' , marginTop:'20px'}}>
                        <Message type="info" bordered showIcon closable><strong>INFO!</strong> Необходимо настроить параметры запроса!</Message>
                       {/* <StepsTutorialComponent/>*/}
                </Stack> 
                </Col> 
             </Row>  
             <Row style={{ marginBottom: '30px', alignItems: 'center' }}>         
                <Col xs={4}>     
                    <Stack className='queryparamscomponentStyle' > 
                        <QueryparamsComponent 
                            queryparams={viewParams} 
                            onParamsChange={handleParamsChange} 
                            setLoadingTable={setLoadingTable} 
                            needsDateParams={needsDateParams} />                                  
                    </Stack>
                </Col>                                
            </Row>
            {loadingTabe === true ?
                (
                   <div className="loading-container">
                        <Placeholder.Paragraph rows={8} />
                       <Loader center content="Загрузка отчета..." vertical size='lg'/>
                   </div>
                ):
                (
                    <div>
                    </div>
                )
            }           
         </>
        ) : (
            <Row>             
            <Col xs={24}>   
                <>
                    <h2 className="query-view-title">{queryviews.title} с {viewParams.startDate} по {viewParams.endDate}</h2>                   
                    <Stack style={{ marginTop:20, textAlign: 'left'}} > 
                            <QueryparamsComponent 
                                queryparams={viewParams} 
                                onParamsChange={handleParamsChange} 
                                setLoadingTable={setLoadingTable}
                                needsDateParams={needsDateParams}/>
                            <div style={{ marginLeft:50}} >
                                <Button appearance="primary" onClick={ ()=>setShowTable(true)} className='custom-button'>
                                    Выгрузить  отчет
                                </Button>
                            </div>
                           
                    </Stack>
                    <FilterComponent
                            headers={columnKeys}
                            selectedColumn={selectedColumn}
                            setSelectedColumn={setSelectedColumn}
                            filterValue={filterValue}
                            setFilterValue={setFilterValue}
                            handleApplyFilter={handleApplyFilter}
                            resetFilter={resetFilter}
                        />  
                    <div style={{ margin: '10px 5px 0px 5px' }}>
                                <TagPicker
                                    data={headers.map(header => ({ label: header, value: header }))}
                                    value={columnKeys}
                                    onChange={setColumnKeys}
                                    block
                                    style={{ marginBottom: 20 }}
                                    cleanable={false}
                                    placeholder="Select columns to display"
                                />
                    </div>                  
                    <div style={{ margin: '0px 5px 0px 5px' }}>
                            {data && data.length > 0 ? 
                            (
                            <Table
                                height={500}
                                data={data}
                                sortColumn={sortColumn}
                                sortType={sortType}
                                onSortColumn={handleSortColumn}
                                loading={loading}
                                autoHeight
                                autoFocus={true}
                                autoCorrect="true"
                            >
                                {headers
                                    .filter(header => columnKeys.includes(header))
                                    .map((header, index) => (
                                        <Column key={index} width={200} align="center" resizable sortable>
                                            <CustomHeaderCell>{header}</CustomHeaderCell>
                                            <Cell dataKey={header} />
                                        </Column>
                                    ))}
                            </Table>
                            ) : (
                            <Row>
                                    <div className="loading-container">
                                        <p>Нет данных для отображения</p>
                                    </div>
                            </Row>
                            )
                            } 
                    </div> 
                    <div style={{ margin: '50px 10px 0px 15px' }}>                                   
                    <Pagination
                                prev
                                next
                                first
                                last
                                ellipsis
                                boundaryLinks
                                maxButtons={5}
                                size="sm"
                                layout={['total', '-', 'limit', '|', 'pager', 'skip']}
                                total={totalCount}
                                limitOptions={[10, 30, 50]}
                                limit={limit}
                                activePage={offset}
                                onChangePage={handlePageChange}
                                onChangeLimit={handleChangeLimit}
                                style={{marginTop: '10px'}}
                    //onChangeLength={handleChangeLimit}
                    /> 
                    </div>  
                    </>   
            </Col>
            </Row>
        )
        }
        </Grid>
        </>
    );
};
export default QueryViewComponent;
