import React from 'react';
import { Accordion, InputPicker,Input, Button } from 'rsuite';


interface FilterProps {
    headers: string[];
    selectedColumn: string;
    setSelectedColumn: (column: string) => void;
    filterValue: string;
    setFilterValue: (value: string) => void;
    handleApplyFilter: () => void;
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
    return (
        <Accordion>
        <Accordion.Panel header="Фильтры поиска"defaultExpanded>
        <div style={{ margin: '10px 0' }}>
      <InputPicker
      data={headers.map(header => ({ label: header, value: header }))}
      value={selectedColumn}
      onChange={value => setSelectedColumn(value)}
      style={{ width: '100%' }}
      placeholder="Выберите столбец"
      className='scrollable-dropdown-menu'
    />
  </div>
  <div style={{ margin: '10px 0' }}>
    <label>Введите значение</label>
    <Input value={filterValue} onChange={setFilterValue} />
  </div>
  <div style={{ margin: '10px 0' }}>
    <Button appearance="primary" onClick={handleApplyFilter}>Применить фильтр</Button>{' '}
    <Button onClick={resetFilter}>Отмена</Button>
  </div>
        </Accordion.Panel>
    </Accordion>
    )
};

export default FilterComponent;