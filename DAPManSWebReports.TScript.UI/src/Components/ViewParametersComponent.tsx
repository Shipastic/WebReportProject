import React, { useState } from 'react';
import { Drawer, ButtonToolbar, IconButton, Button, FlexboxGrid, Panel, Form } from 'rsuite';
import AngleUpIcon from '@rsuite/icons/legacy/AngleUp';
import 'rsuite/dist/rsuite.css';

interface ViewParams {
    startDate: string;
    endDate:   string
}
interface Props {
    queryparams: ViewParams;
    onParamsChange: (params: ViewParams) => void;
}

const ViewParametersComponent: React.FC<Props> = ({ queryparams, onParamsChange }) => {
    const [params, setParams] = useState<ViewParams>(queryparams);

    const [size, setSize]           = useState('xs');
    const [open, setOpen]           = useState(true);
    const [placement, setPlacement] = useState('top');
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate]     = useState('');

    const handleChage = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setParams(prev => ({ ...prev, [name]: value }));
    };
    const handleSubmit = () => {
        onParamsChange(params);
        setOpen(false);
    };
    const handleOpen = (key) =>
    {
        setOpen(true);
        setPlacement(key);
    };
  return (
      <div>
          <ButtonToolbar>
              <IconButton icon={<AngleUpIcon />} onClick={() => handleOpen('top')}>
                  Query Parameters
              </IconButton>
          </ButtonToolbar>
          <Drawer size={size} placement={placement} open={open} onClose={() => setOpen(false)} >             
              <Drawer.Body >
                  <FlexboxGrid justify="space-around">
                  <FlexboxGrid.Item colspan={4}>
                      <Panel header={<h4>Select Date</h4>} bordered>
                          <Form fluid>
                              <Form.Group>
                                  <Form.ControlLabel>Start Date Time</Form.ControlLabel>
                                  <input
                                      type="date"
                                      id="startDate"
                                      name="startDate"
                                      value={params.startDate}
                                      onChange={handleChage}
                                  />
                              </Form.Group>
                              <Form.Group>
                                  <Form.ControlLabel>End Date Time</Form.ControlLabel>
                                  <input
                                          type="date"
                                          id="endDate"
                                          name="endDate"
                                          value={params.endDate}
                                          onChange={handleChage}
                                  />
                              </Form.Group>                             
                          </Form>
                      </Panel>                      
                  </FlexboxGrid.Item> 
                  <FlexboxGrid.Item colspan={4}>
                      <Panel header={<h4>Select Others Params</h4>} bordered>
                          <Form fluid>
                              <Form.Group>

                              </Form.Group>
                          </Form>
                      </Panel>
                      </FlexboxGrid.Item>
                  </FlexboxGrid>
                  <Drawer.Actions>
                      <Button onClick={() =>  setOpen(false) } appearance="default" >
                          Cancel
                      </Button>
                      <Button onClick={handleSubmit} appearance="primary">
                          Confirm
                      </Button>
                  </Drawer.Actions>
              </Drawer.Body>
          </Drawer>
     </div>
  );
}

export default ViewParametersComponent;