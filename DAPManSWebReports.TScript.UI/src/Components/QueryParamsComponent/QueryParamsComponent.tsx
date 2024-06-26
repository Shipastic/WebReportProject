import React, { useState, useEffect } from 'react';
import { IconButton, ButtonToolbar, SelectPicker, InputNumber, Button, Form, Drawer, DateRangePicker, Message, FlexboxGrid } from 'rsuite';
import SettingIcon from '@rsuite/icons/Setting';
import CalendarIcon from '@rsuite/icons/Calendar';
import formatDateForOracle from '../../Utils/formatDateForOracle';
import './QueryParams.css';

interface ViewParams 
{
    startDate: string;
    endDate: string;
    presetDate: string;
    format: string;
    sortOrder: string;
    sortColumnNumber: number;
}
interface Props 
{
    queryparams: ViewParams;
    onParamsChange: (params: ViewParams) => void;
    setLoadingTable: (loading: boolean) => void;
    needsDateParams: boolean;
    queryTitle: string;
    fetchdata: (dataviewid: number | null, offset: number, limit: number, viewParams: ViewParams) => void;
    dataviewid: number | null;
    offset: number;
    limit: number;
}

const getPresetDates = (preset: string) => {
    const today = new Date();
    let startDate = today;
    let endDate = today;

    switch (preset) {
        case 'today':
            startDate.setHours(0, 0, 0, 0);
            endDate.setHours(23, 59, 59, 999);
            break;
        case 'yesterday':
            startDate.setDate(today.getDate() - 1);
            startDate.setHours(0, 0, 0, 0);
            endDate.setDate(today.getDate() - 1);
            endDate.setHours(23, 59, 59, 999);
            break;
        case 'week':
            startDate.setDate(today.getDate() - today.getDay() + 1);
            startDate.setHours(0, 0, 0, 0);
            endDate.setDate(startDate.getDate() + 6);
            endDate.setHours(23, 59, 59, 999);
            break;
        case 'month':
            startDate = new Date(today.getFullYear(), today.getMonth(), 1);
            startDate.setHours(0, 0, 0, 0);
            endDate   = new Date(today.getFullYear(), today.getMonth() + 1, 0);
            endDate.setHours(23, 59, 59, 999);
            break;
        default:
            break;
    }

    return {
        startDate: formatDateForOracle(startDate.toISOString()),
        endDate:   formatDateForOracle(endDate.toISOString())
    };
};

const QueryparamsComponent: React.FC<Props> = ({ queryparams, onParamsChange, setLoadingTable, needsDateParams, queryTitle, fetchdata, dataviewid, offset, limit }) => {
    const [params, setParams] = useState<ViewParams>(queryparams);

    const [drawerOpen, setDrawerOpen] = useState(true);

    useEffect(() => {
        if (!needsDateParams) {
            setParams((prevParams) => ({
                ...prevParams,
                startDate: '',
                endDate: '',
            }));
        }
    }, [needsDateParams]);

    const handleDateRangeChange = (value: [Date | null, Date | null]) => {
        if (!(value[0] && value[1])) return;

        setParams({
            ...params,
            startDate: needsDateParams ? formatDateForOracle(value[0].toISOString()) : '',
            endDate: needsDateParams ? formatDateForOracle(value[1].toISOString()) : ''
        });
    };

    const handleSubmit = () => {
        onParamsChange(params);
        fetchdata(dataviewid, offset, limit, params); 
        setDrawerOpen(false);
        setLoadingTable(true);
        console.log(params);
    };

    const handleChange = (key: string, value: any) => {
        setParams({ ...params, [key]: value });
    };

    const handlePresetDateChange = (value: string) => {
        const { startDate, endDate } = getPresetDates(value);
        setParams({ ...params, presetDate: value, startDate, endDate });
    };

    const presetDates = [
        { label: 'Сегодня', value: 'Today' },
        { label: 'Вчера', value: 'Yesterday' },
        { label: 'Прошлая неделя', value: 'week' },
        { label: 'Прошлый месяц', value: 'month' }
    ];

    const formats = [
        { label: 'CSV', value: 'CSV' },
        { label: 'Excel', value: 'Excel' },
        { label: 'PDF', value: 'PDF' },
    ];

    const sortOrders = [
        { label: 'По возрастанию', value: 'Ascending' },
        { label: 'По убыванию', value: 'Descending' },
    ];

    return (
        <>
           <ButtonToolbar>
                <IconButton icon={<SettingIcon className='menu-icon-button'/>} onClick={() => setDrawerOpen(true)}  className='text-icon-button'>
                    Параметры запроса
                </IconButton>
            </ButtonToolbar>
            <Drawer
                open={drawerOpen}
                onClose={() => setDrawerOpen(false)}
                placement="right"
                size="xs"
                className="custom-drawer"
            >
                <Drawer.Header>
                    <Drawer.Title>Параметры запроса</Drawer.Title>
                </Drawer.Header>
                <Drawer.Body>
                    <Form layout="vertical" onSubmit={handleSubmit} autoCorrect='true' >
                        {needsDateParams ? (
                            <>
                                <Form.Group>
                                    <Form.ControlLabel>Предустановленные фильтры дат:</Form.ControlLabel>
                                    <SelectPicker data={presetDates} onChange={handlePresetDateChange} placeholder="Выберите предустановленную дату" value={params.presetDate} />
                                </Form.Group>
                                <Form.Group>
                                    <Form.ControlLabel>Диапазон дат:</Form.ControlLabel>
                                    <DateRangePicker
                                        className="custom-date-picker"
                                        value={[new Date(params.startDate), new Date(params.endDate)]}
                                        onChange={handleDateRangeChange}
                                        format="yyyy-MM-dd'T'HH:mm:ssXXX"
                                        caretAs={CalendarIcon}
                                        showMeridian
                                        block
                                        appearance="subtle"
                                        style={{ width: 130 }}
                                        size="sm"
                                        placeholder="Выберите диапазон дат"
                                        showOneCalendar
                                    />
                                </Form.Group>
                            </>
                        ) : (
                            <Form.Group>
                                <Message type="info">Для данного представления выбор дат недоступен!</Message>

                            </Form.Group>
                        )}
                        <Form.Group>
                            <FlexboxGrid>
                                <span>
                                <Form.ControlLabel>Формат:</Form.ControlLabel>
                                <SelectPicker data={formats} value={params.format} onChange={(value) => handleChange('format', value)} />
                                </span>
                                <span style={{marginLeft:'80px'}}>
                                <Form.ControlLabel >Порядок сортировки:</Form.ControlLabel>
                                <SelectPicker data={sortOrders} value={params.sortOrder} onChange={(value) => handleChange('sortOrder', value)} />
                                </span>
                                </FlexboxGrid>
                        </Form.Group>
                        <Form.Group>
                          
                        </Form.Group>
                        <Form.Group>
                            <Form.ControlLabel>Столбец для сортировки:</Form.ControlLabel>
                            <InputNumber min={1} value={params.sortColumnNumber} onChange={(value) => handleChange('sortColumnNumber', value)} className="custom-input-number" />
                        </Form.Group>
                        <Form.Group style={{width:'360px'}}>
                            <Button appearance="primary" onClick={handleSubmit} className="custom-button">Вывести результат</Button>
                        </Form.Group>
                    </Form>
                </Drawer.Body>
            </Drawer>
        </>
    );
};

export default QueryparamsComponent;