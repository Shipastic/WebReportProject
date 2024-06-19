import React, { useState } from 'react';
import { Accordion, InputPicker, Input, Button, IconButton, Drawer, Form } from 'rsuite';
import Filter from '@rsuite/icons/legacy/Filter';
import '../App.css';


interface FilterProps 
{
  headers: string[];
  selectedColumn: string;
  setSelectedColumn: (column: string) => void;
  filterValue: string;
  setFilterValue: (value: string) => void;
  handleApplyFilter: (filters) => void;
  resetFilter: () => void;
}

const FilterComponent: React.FC<FilterProps>= ({
    headers,
    selectedColumn,
    setSelectedColumn,
    filterValue,
    setFilterValue,
    handleApplyFilter,
    resetFilter
}) => {
    const [filters, setFilters] = useState([{ column: '', value: '' }]);

    const [drawerOpen, setDrawerOpen] = useState(false);

    const openDrawer = () => setDrawerOpen(true);
    const closeDrawer = () => setDrawerOpen(false);

    const addFilter = () => {
        setFilters([...filters, { column: '', value: '' }]);
      };

      const updateFilter = (index, key, value) => {
        const newFilters = filters.slice();
        newFilters[index][key] = value;
        setFilters(newFilters);
      };

    const applyFilterAndClose = () => {
        handleApplyFilter(filters);
        closeDrawer();
    };

    const resetFilterAndClose = () => {
        resetFilter();
        closeDrawer();
    };

    const footerButtonStyleConfirm = {
        float: 'left',
        marginRight: 10,
        marginTop: 2
    };
    const footerButtonStyleCancel = {
        float: 'right',
        marginRight: 10,
        marginTop: 2
    };

    return (
        <div>
            <IconButton icon={<Filter className='menu-icon-button' style={{ backgroundColor: '#263444' }} />} onClick={openDrawer} style={{ backgroundColor: '#4e6b8b', color: 'white' }} className='menu-icon-button'>
                Фильтр
            </IconButton>
            <Drawer open={drawerOpen} onClose={closeDrawer} placement="right" className="custom-drawer" style={{ width:400 }}>
                <Drawer.Header>
                    <Drawer.Title>Фильтр поиска</Drawer.Title>
                </Drawer.Header>
                <Drawer.Body>
                    <Form layout="vertical" onSubmit={applyFilterAndClose} autoCorrect='true' >
                    {filters.map((filter, index) => (
                        <Form.Group key={index}>
                        <div style={{ margin: '10px 0' }}>
                        <InputPicker
                            data={headers.map(header => ({ label: header, value: header }))}
                            value={selectedColumn}
                            onChange={value => updateFilter(index, 'column', value)}
                            style={{ width: '100%' }}
                            placeholder="Выберите столбец"
                            className='scrollable-dropdown-menu'
                        />
                        </div>
                        <div style={{ margin: '10px 0' }}>
                        <Form.ControlLabel>Введите значение</Form.ControlLabel>
                        <Input value={filter.value} onChange={value => updateFilter(index, 'value', value)} />
                            </div>
                        </Form.Group>
                        ))}
                         <Form.Group>
                            <div style={{ margin: '10px 0' }}>
                                <Button onClick={addFilter}>Добавить фильтр</Button>
                            </div>
                        </Form.Group>
                        <Form.Group>
                            <div style={{ margin: '10px 0' }}>
                                <Button appearance="primary" onClick={applyFilterAndClose} style={footerButtonStyleConfirm}>
                            Применить фильтр
                                </Button>
                                <Button onClick={resetFilterAndClose} style={footerButtonStyleCancel}>Сбросить фильтр</Button>
                            </div>
                        </Form.Group>
                    </Form>
                </Drawer.Body>
            </Drawer>
        </div>
    )
};

export default FilterComponent;