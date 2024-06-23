import React, { useEffect, useState, useRef } from 'react';
import { QueryViewDTO } from '../Models/QueryViewDTO';
import { Placeholder, Table, Pagination, Button, Row, Col, Grid, Stack, Message, Loader, CheckPicker, Checkbox} from 'rsuite';
import QueryparamsComponent from './QueryParamsComponent';
import FilterComponent from './FilterComponent';
import ExcelDataComponent from './ExcelDataComponent';
import 'rsuite-table/dist/css/rsuite-table.css';
import '../App.css';

const { Column, HeaderCell, Cell } = Table;
interface Props {
    dataviewid: number | null;
    path: string;
    updateBreadcrumbs: (breadcrumbs: string[]) => void;
    breadcrumbs: string[];
}
interface ViewParams {
    startDate: string;
    endDate: string;
    presetDate: string;
    format: string;
    sortOrder: string;
    sortColumnNumber: number;
}
const CustomHeaderCell = ({ ...props }) => (
    <HeaderCell {...props} style={{ backgroundColor: '#4CAF50', color: 'white', fontWeight: 'bold', padding: '4px 0 2px 0' }} />
);

const QueryViewComponent: React.FC<Props> = ({ dataviewid, path, updateBreadcrumbs, breadcrumbs }) => {
    const [queryviews, setData                ] = useState<QueryViewDTO | null>(null);
    const [loading, setLoading                ] = useState<boolean>(false);
    const [loadingTable, setLoadingTable      ] = useState<boolean>(false);   
    const [sortColumn, setSortColumn          ] = useState<string | null>(null);
    const [sortType, setSortType              ] = useState<'asc' | 'desc' | undefined>();
    const [error, setError                    ] = useState<string | null>(null);
    const [columnKeys, setColumnKeys          ] = useState<string[]>([]);
    const [viewParams, setViewParams          ] = useState<ViewParams>({ startDate: '', endDate: '', presetDate: '', format: 'EXCEL', sortOrder: 'asc', sortColumnNumber: 1});
    const [showTable, setShowTable            ] = useState<boolean>(false);
    const [titleView, setTitleView            ] = useState<string | null>('report');
    const [filterValue, setFilterValue        ] = useState('');
    const [filteredData, setFilteredData      ] = useState<Array<{ [key: string]: any }>>([]);
    const [selectedColumn, setSelectedColumn  ] = useState('');
    const [needsDateParams, setNeedsDateParams] = useState<boolean>(false);
    const [queryResult, setqueryResult        ] = useState<string | null>(null);
    const [isFilterApplied, setIsFilterApplied] = useState<boolean>(false);
    const [isAllSelected, setIsAllSelected    ] = useState<boolean>(false);

    const [prev, setPrev                      ] = useState(true);
    const [next, setNext                      ] = useState(true);
    const [first, setFirst                    ] = useState(true);
    const [last, setLast                      ] = useState(true);
    const [ellipsis, setEllipsis              ] = useState(true);
    const [layout, setLayout                  ] = useState(['total', '-', 'limit', '|', 'pager', 'skip']);
    const [limit, setLimit                    ] = useState(12);
    const [offset, setPage                    ] = useState(1);
    const [size, setSize                      ] = useState('sm');
    const [maxButtons, setMaxButtons          ] = useState(5);
    const [totalCount, setTotalCount          ] = useState(0);
    const [boundaryLinks, setBoundaryLinks    ] = useState(true);

    const [queryparamsComponentCalls, setQueryparamsComponentCalls] = useState(0);

    const picker = useRef();

    useEffect(() => {
        if (dataviewid) {
            setLoading(true);
            checkColumnPresence(dataviewid);
        }
    }, [dataviewid]);
    
    useEffect(() => {
       if(queryparamsComponentCalls > 0){
        setLoading(true);
        fetchdata(dataviewid, offset, limit, viewParams);
       }
}, [offset, limit, sortColumn, sortType, dataviewid, viewParams]);

    const checkColumnPresence = async (dataviewid: number | null) => {
        try {
            const response = await fetch(`https://localhost:7263/api/dataview/${dataviewid}`);
            const result = await response.json();
            setNeedsDateParams(result.startDateField !== '' && result.endDateField !== '');
        }
        catch (error: any) {
            setError(error.message);
        }
        finally {
            setLoading(false);
        }
    };
    const fetchdata = async (dataviewid: number | null, offset: number, limit: number, viewParams: ViewParams) =>
    {
        if (dataviewid === null) return;
        try {
            const params = new URLSearchParams({
                limit: limit.toString(),
                offset: offset.toString(),
                startDate: viewParams.startDate,
                endDate: viewParams.endDate,
                presetDate: viewParams.presetDate,
                format: viewParams.format,
                sortOrder: sortType,
                sortColumnNumber: sortColumn,              
            });
            const response = await fetch(`https://localhost:7263/api/query/${dataviewid}?${params.toString()}`);
            if (!response.ok) {
                throw new Error(`Error: ${response.status}`);
            }
            const result = await response.json();
            const headers = result.result.length > 0 ? Object.keys(result.result[0]) : [];
            setData(result);
            setTotalCount(result.totalCount);
            setPage(result.offset);
            setLimit(result.pageSize);
            setqueryResult(result.queryResult);
            setColumnKeys(headers);
            setShowTable(result.result.length > 0);
            setFilteredData(result.pagedItems);
            setTitleView(result.title);
        }
        catch (error: any) {
            setError(error.message);
        }
        finally {
            setLoading(false);
            setLoadingTable(false);
        }
    };
    const handlePageChange = (offset: React.SetStateAction<number>) => {
        setPage(offset);
    };
    const handleParamsChange = (params: ViewParams) => {
        setViewParams(params);
        setQueryparamsComponentCalls(queryparamsComponentCalls + 1)
        fetchdata(dataviewid, offset, limit, params);

    };

    const getData = () => {
        if (!queryviews || !filteredData) return [];
        return queryviews.result;
    };

    const data = getData();

    const headers = data.length > 0 ? Object.keys(data[0]) : [];

    const handleChangeLimit = (limit) => {
        setPage(offset );
        setLimit(limit);
    };

    const handleSortColumn = (sortColumn: string, sortType: 'asc' | 'desc') => {
        setSortColumn(sortColumn);
        setSortType(sortType);

    };
  
    const handleApplyFilter = (filters) => {
        let filtered = queryviews?.result;
    
        filters.forEach(filter => {
            if (filter.column && filter.value) {
                filtered = filtered?.filter(item => {
                    const value = item[filter.column];
                    if (typeof value === 'number') {
                        return value.toString() === filter.value;
                    }
                    return value !== undefined && value.toString().toLowerCase().includes(filter.value.toLowerCase());
                });
            }
        });
    
        setFilteredData(filtered);
        setIsFilterApplied(true);
    };
    const resetFilter = () => {
        setFilteredData(queryviews?.pagedItems || []);
        setIsFilterApplied(false);
    };

    const toggleSelectAll = (checked: boolean) => {
        if (checked) {
            setColumnKeys(headers);
            setIsAllSelected(true);
        } else {
            setColumnKeys([]);
            setIsAllSelected(false);
        }
    };

    const availableColumns = headers.map(header => ({ label: header, value: header }));

    const footerButtonStyle = {
        float: 'right',
        marginRight: 10,
        marginTop: 2
    };
    return (
        <>
            <Grid fluid style={{marginRight:'5px'}}>
                {!showTable ?
                    (<>
                        <Row>
                            <Col xs={12}>
                                <Stack style={{ textAlign: 'left', marginTop: '20px' }}>
                                    <Message type="info" bordered showIcon closable><strong>INFO!</strong> Необходимо настроить параметры запроса!</Message>
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
                                        needsDateParams={needsDateParams} 
                                        queryTitle={titleView}     
                                        fetchdata={fetchdata}  
                                        dataviewid={dataviewid}          
                                        offset={offset}              
                                        limit={limit}
                                        />   
                                </Stack>
                            </Col>
                        </Row>
                        {loadingTable === true ?
                            (
                                <div className="loading-container">
                                    <Loader center content="Загрузка отчета..." vertical size='lg' />
                                </div>
                            ) :(<div></div>)
                        }
                    </>
                    ) : (
                        <Row>
                            <Col xs={24}>
                                <>
                                    <h2 className="query-view-title">
                                        {queryviews?.title}
                                        {viewParams.startDate && viewParams.endDate && ` с ${viewParams.startDate} по ${viewParams.endDate}`}
                                    </h2>
                                    <Stack style={{ marginTop: 20, justifyContent: 'space-between', alignItems: 'center' }} >
                                        <div style={{ display: 'flex', alignItems: 'center', gap: '10px', flexGrow: 1 }}>
                                            <QueryparamsComponent
                                                queryparams={viewParams}
                                                onParamsChange={handleParamsChange}
                                                setLoadingTable={setLoadingTable}
                                                needsDateParams={needsDateParams} 
                                                queryTitle={titleView}
                                                fetchdata={fetchdata}  
                                                dataviewid={dataviewid}          
                                                offset={offset}              
                                                limit={limit}                                          
                                            /> 
                                            <FilterComponent
                                                headers={headers}
                                                selectedColumn={selectedColumn}
                                                setSelectedColumn={setSelectedColumn}
                                                filterValue={filterValue}
                                                setFilterValue={setFilterValue}
                                                handleApplyFilter={handleApplyFilter}
                                                resetFilter={resetFilter}
                                            />
                                            <div>
                                                <label style={{marginLeft:'20px', fontSize:'16px', color:'white'} }>Выбор отображаемых столбцов</label>
                                                <CheckPicker
                                                    value={columnKeys}
                                                    onChange={setColumnKeys}
                                                    block
                                                    style={{ marginBottom: 20, width: 300 }}
                                                    menuStyle={{ width: 300 }}
                                                    placeholder="Выбор столбцлв для вывода в таблице"
                                                    cleanable={false}
                                                    data={availableColumns}
                                                    searchable={true}
                                                    ref={picker}
                                                    renderExtraFooter={() => (
                                                        <div style={{ marginBottom: 20 }}>
                                                            <Checkbox onChange={(value, checked) => toggleSelectAll(checked)}>
                                                                Select All
                                                            </Checkbox>
                                                            <Button
                                                                style={footerButtonStyle}
                                                                appearance="primary"
                                                                size="sm"
                                                                onClick={() => {
                                                                    picker.current.close();
                                                                }}
                                                            >
                                                                Ok
                                                            </Button>
                                                        </div>
                                                    )}
                                                />
                                            </div>
                                            <ExcelDataComponent
                                                dataviewid={dataviewid}
                                                queryparams={viewParams}
                                                title={titleView }
                                            />
                                        </div>
                                    </Stack>
                                    <div style={{ margin: '0px 5px 0px 5px' }}>
                                        {isFilterApplied && (
                                            <div style={{ margin: '10px 0', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                                                <span>Найдено совпадений: {filteredData.length}</span>
                                                <Button onClick={resetFilter}>Сбросить фильтр</Button>
                                            </div>
                                        )}
                                        {data && data.length > 0 ?
                                            (
                                                <Table
                                                    height={500}
                                                    data={filteredData}
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
                                                                <Cell dataKey={header}/>
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
                                            prev={prev}
                                            next={next}
                                            first={first}
                                            last={last}
                                            ellipsis={ellipsis}
                                            boundaryLinks={boundaryLinks}
                                            maxButtons={maxButtons}
                                            size={size}
                                            layout={layout}
                                            total={totalCount}
                                            limitOptions={[12, 30, 50]}
                                            limit={limit}
                                            activePage={offset}
                                            onChangePage={handlePageChange}
                                            onChangeLimit={handleChangeLimit}
                                            style={{ marginTop: '10px' }}
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
