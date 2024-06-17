import React, { useState, useEffect } from 'react';
import { IconButton, ButtonToolbar, SelectPicker, InputNumber, Button, Form, Drawer, DateRangePicker, Message } from 'rsuite';
import SettingIcon from '@rsuite/icons/Setting';
import '../App.css';
import CalendarIcon from '@rsuite/icons/Calendar';
import formatDateForOracle from '../Utils/formatDateForOracle';

interface ViewParams {
    startDate: string;
    endDate: string;
    presetDate: string;
    format: string;
    sortOrder: string;
    sortColumnNumber: number;
}
interface Props {
    queryparams: ViewParams;
    onParamsChange: (params: ViewParams) => void;
    setLoadingTable: (loading: boolean) => void;
    needsDateParams: boolean;
    queryTitle: string;
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
            endDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
            endDate.setHours(23, 59, 59, 999);
            break;
        default:
            break;
    }

    return {
        startDate: formatDateForOracle(startDate.toISOString()),
        endDate: formatDateForOracle(endDate.toISOString())
    };
};

const QueryparamsComponent: React.FC<Props> = ({ queryparams, onParamsChange, setLoadingTable, needsDateParams, queryTitle }) => {
    const [params, setParams] = useState<ViewParams>(queryparams);

    const [drawerOpen, setDrawerOpen] = useState(false);

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

    const handleStartDateChange = (date: Date | null) => {
        if (date) {
            setParams({ ...params, startDate: formatDateForOracle(date.toISOString()) });
        }
    };

    const handleEndDateChange = (date: Date | null) => {
        if (date) {
            setParams({ ...params, endDate: formatDateForOracle(date.toISOString()) });
        }
    };

    const handleSubmit = () => {
        onParamsChange(params);
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
        { label: 'Today', value: 'Today' },
        { label: 'Yesterday', value: 'Yesterday' },
        { label: 'Week', value: 'week' },
        { label: 'Month', value: 'month' }
    ];

    const formats = [
        { label: 'CSV', value: 'CSV' },
        { label: 'Excel', value: 'Excel' },
        { label: 'PDF', value: 'PDF' },
    ];

    const sortOrders = [
        { label: 'Ascending', value: 'Ascending' },
        { label: 'Descending', value: 'Descending' },
    ];

    return (
        <>
            <ButtonToolbar>
                <IconButton icon={<SettingIcon color='#0000CD' />} onClick={() => setDrawerOpen(true)} className='menu-icon-button' style={{ backgroundColor: '#4e6b8b', color: 'white' }}>
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
                                    <SelectPicker data={presetDates} onChange={handlePresetDateChange} placeholder="Select a date preset" value={params.presetDate} />
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
                                        placeholder="Select Date Range"
                                        showOneCalendar
                                    />
                                </Form.Group>
                            </>
                        ) : (
                            <Form.Group>
                                <Message type="info">Для данного представления "{queryTitle}" выбор дат недоступен!</Message>

                            </Form.Group>
                        )}
                        <Form.Group>
                            <Form.ControlLabel>Формат:</Form.ControlLabel>
                            <SelectPicker data={formats} value={params.format} onChange={(value) => handleChange('format', value)} />
                        </Form.Group>
                        <Form.Group>
                            <Form.ControlLabel>Порядок сортировки:</Form.ControlLabel>
                            <SelectPicker data={sortOrders} value={params.sortOrder} onChange={(value) => handleChange('sortOrder', value)} />
                        </Form.Group>
                        <Form.Group>
                            <Form.ControlLabel>Столбец для сортировки:</Form.ControlLabel>
                            <InputNumber min={1} value={params.sortColumnNumber} onChange={(value) => handleChange('sortColumnNumber', value)} className="custom-input-number" />
                        </Form.Group>
                        <Form.Group>
                            <Button appearance="primary" onClick={handleSubmit} className="custom-button">Вывести результат</Button>
                        </Form.Group>
                    </Form>
                </Drawer.Body>
            </Drawer>
        </>
    );
};

export default QueryparamsComponent;