import React,{ useEffect, useState } from 'react';
import { QueryViewDTO } from '../Models/QueryViewDTO';
import '../App.css';
import '../App.scss';
import { Table, Pagination, TagPicker, Button } from 'rsuite';
import ViewParametersComponent from '../Components/ViewParametersComponent';


const { Column, HeaderCell, Cell } = Table;

interface Props {
    dataviewid: number | null;
    path: string;
    updateBreadcrumbs: (breadcrumbs: string[]) => void;
    breadcrumbs: string[];
}
interface ViewParams {
    startDate: string;
    endDate: string 
}

interface PagedResult<T> {
    items: T[];
    totalCount: number;
    offset: number;
    pageSize: number;
}

const QueryViewComponent: React.FC<Props> = ({ dataviewid, path, updateBreadcrumbs, breadcrumbs }) => {
    const [queryviews, setData] = useState<QueryViewDTO | null>(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [offset, setPage] = useState(1);
    const [limit, setLimit] = useState(10);
    const [totalCount, setTotalCount] = useState(0);
    const [sortColumn, setSortColumn] = useState<string | null>(null);
    const [sortType, setSortType] = useState<'asc' | 'desc' | undefined>();
    const [error, setError] = useState<string | null>(null);
    const [columnKeys, setColumnKeys] = useState<string[]>([]);

    const [viewParams, setViewParams] = useState<ViewParams>({ startDate: '', endDate: '' });
    const [showTable, setShowTable] = useState<boolean>(false);  

    useEffect(() => {
        if (viewParams.startDate && viewParams.endDate)
        {
            setLoading(true);
            fetchdata(dataviewid, offset, limit, viewParams);
        }
    }, [dataviewid, offset, limit, viewParams]);

    const fetchdata = async (dataviewid: number | null, offset: number, limit: number, viewParams: ViewParams) => {
        if (dataviewid === null) return;
        try
        {                   
            //const response = await fetch(`https://localhost:7263/api/query/${dataviewid}?limit=${limit}&offset=${offset}`);  
            const response = await fetch(`https://localhost:7263/api/query/${dataviewid}?limit=${limit}&offset=${offset}&startDate=${viewParams.startDate}&endDate=${viewParams.endDate}`);
           if (!response.ok)
           {
               throw new Error(`Error: ${response.status}`);
           }
           const result = await response.json();
            //setData(result);
            setData(result);
            setTotalCount(result.totalCount);
            setPage(result.offset);
            setLimit(result.pageSize);
           const headers = result.result.length> 0 ? Object.keys(result.result[0]) : [];
            setColumnKeys(headers);
            setShowTable(result.result.length > 0);
        }
        catch (error: any)
        {
            setError(error.message);
        }
        finally
        {
            setLoading(false);
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
   
    return (
        <div className="query-view-container">
            {!showTable ? (
                <div style={{ padding: '0px 0px 20px 75px', display: 'flex', alignItems: 'center' }}>
                    <ViewParametersComponent queryparams={viewParams} onParamsChange={handleParamsChange} />
                    <span style={{ margin: ' 0 0 0 10px' }}>
                        <Button onClick={ ()=>setShowTable(true)} appearance="primary" >
                            Execute Query
                        </Button>
                    </span>
                </div>
            ) : (
                <>
                    {error && <p style={{ color: 'red' }}>{error}</p>}
                    <h2 className="query-view-title">{queryviews.title}</h2>
                    <div style={{ padding: '0px 0px 20px 75px' }}>
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
                    <div style={{ padding: '0px 0px 20px 75px' }}>
                        {data && data.length > 0 ? (
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
                                            <HeaderCell>{header}</HeaderCell>
                                            <Cell dataKey={header} />
                                        </Column>
                                    ))}
                            </Table>
                        ) : (
                            <p>No data available</p>
                        )}
                    </div>
                    <div style={{ padding: '0px 0px 20px 75px' }}>
                        <Pagination
                            prev
                            next
                            first
                            last
                            ellipsis
                            boundaryLinks
                            maxButtons={5}
                            size="xs"
                            layout={['total', '-', 'limit', '|', 'pager', 'skip']}
                            total={totalCount}
                            limitOptions={[10, 30, 50]}
                            limit={limit}
                             activePage={offset}
                            onChangePage={handlePageChange}
                            onChangeLimit={handleChangeLimit}
                            //onChangeLength={handleChangeLimit}
                        />
                    </div>
                </>
            )}
        </div>
    );
};
export default QueryViewComponent;
