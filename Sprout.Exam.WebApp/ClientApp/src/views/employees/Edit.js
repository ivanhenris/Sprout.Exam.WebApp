import React, { Component } from 'react';
import authService from '../../components/api-authorization/AuthorizeService';

export class EmployeeEdit extends Component {
  static displayName = EmployeeEdit.name;

  constructor(props) {
    super(props);
    this.state = { id: 0,fullName: '',birthdate: '',tin: '',typeId: 1, loading: true,loadingSave:false, employeetypes: [], basePay: 0, label: '', errors: []};
  }

  async componentDidMount() {
    await this.getEmployee(this.props.match.params.id);
    await this.populateEmployeeTypes();
    await this.getEmployeeTypeLabel(this.state.typeId);
  }

  handleChange(event) {
    this.setState({ [event.target.name] : event.target.value});
    this.setState({ errors: [] });
  }

  async handleddlChange(event) {
    await this.setState({ [event.target.name] : event.target.value});
    this.setState({ errors: [] });
    await this.getEmployeeTypeLabel(this.state.typeId);
  }

  handleSubmit(e){
      const value = this.state.basePay;
      let basePayCheck = !isNaN(+value);
      if (!this.state.fullName || !this.state.birthdate || !this.state.tin || !this.state.basePay) {
        e.preventDefault();
        this.fieldHighlighting();
        alert('Please fill all required fields.');
        return;
      }
      else if(basePayCheck === false){
        e.preventDefault();
        alert('Pay input is invalid');
        return;
      }
      e.preventDefault();
      if (window.confirm("Are you sure you want to save?")) {
        this.saveEmployee();
      } 
  }

  fieldHighlighting() {
    let errors = [];
    let {fullName, birthdate, tin, basePay} = this.state;
    if(!fullName) {
      errors.push('fullName');
    }
    if(!birthdate) {
      errors.push('birthdate');
    }
    if(!tin) {
      errors.push('tin');
    }
    if(!basePay) {
      errors.push('basePay');
    }
    if (errors.length) { 
      this.setState({ errors });
      return;
    }
  }

  render() {
    let types = this.state.employeetypes;
    let optionItems = types.map((types) =>
                <option key={types.id} value={types.id}>{types.typeName}</option>
            );

    let contents = this.state.loading
    ? <p><em>Loading...</em></p>
    : <div>
    <form>
<div className='form-row'>
<div className='form-group col-md-6'>
  <label htmlFor='inputFullName4'>Full Name: *</label>
  <input type='text' className='form-control' id='inputFullName4' onChange={this.handleChange.bind(this)} name="fullName" value={this.state.fullName} placeholder='Full Name' style={{ borderColor: this.state.errors.includes('fullName') ? 'red' : 'LightGray' }}/>
</div>
<div className='form-group col-md-6'>
  <label htmlFor='inputBirthdate4'>Birthdate: *</label>
  <input type='date' className='form-control' id='inputBirthdate4' onChange={this.handleChange.bind(this)} name="birthdate" value={this.state.birthdate} placeholder='Birthdate' style={{ borderColor: this.state.errors.includes('birthdate') ? 'red' : 'LightGray' }}/>
</div>
</div>
<div className="form-row">
<div className='form-group col-md-6'>
  <label htmlFor='inputTin4'>TIN: *</label>
  <input type='text' className='form-control' id='inputTin4' onChange={this.handleChange.bind(this)} value={this.state.tin} name="tin" placeholder='TIN' style={{ borderColor: this.state.errors.includes('tin') ? 'red' : 'LightGray' }}/>
</div>
<div className='form-group col-md-6'>
  <label htmlFor='inputEmployeeType4'>Employee Type: *</label>
  <select id='inputEmployeeType4' onChange={this.handleddlChange.bind(this)} value={this.state.typeId}  name="typeId" className='form-control'>
    {optionItems}
  </select>
</div>
</div>
<div className='form-row'>
<div className='form-group col-md-6'>
  <label htmlFor='inputBasePay4'>{this.state.label}: *</label>
  <input type='text' className='form-control' id='inputBasePay4' onChange={this.handleChange.bind(this)} name="basePay" value={this.state.basePay} style={{ borderColor: this.state.errors.includes('fullName') ? 'red' : 'LightGray' }}/>
</div>
</div>
<button type="submit" onClick={this.handleSubmit.bind(this)} disabled={this.state.loadingSave} className="btn btn-primary mr-2">{this.state.loadingSave?"Loading...": "Save"}</button>
<button type="button" onClick={() => this.props.history.push("/employees/index")} className="btn btn-primary">Back</button>
</form>
</div>;


    return (
        <div>
        <h1 id="tabelLabel" >Employee Edit</h1>
        <p>All fields are required</p>
        {contents}
      </div>
    );
  }

  async saveEmployee() {
    this.setState({ loadingSave: true });
    const token = await authService.getAccessToken();
    const requestOptions = {
        method: 'PUT',
        headers: !token ? {} : { 'Authorization': `Bearer ${token}`,'Content-Type': 'application/json' },
        body: JSON.stringify(this.state)
    };
    const response = await fetch('api/employees/' + this.state.id,requestOptions);

    if(response.status === 200){
        this.setState({ loadingSave: false });
        alert("Employee successfully saved");
        this.props.history.push("/employees/index");
    }
    else{
        alert("There was an error occured.");
    }
  }

  async getEmployee(id) {
    this.setState({ loading: true,loadingSave: false });
    const token = await authService.getAccessToken();
    const response = await fetch('api/employees/' + id, {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ id: data.id,fullName: data.fullName,birthdate: data.birthdate,tin: data.tin,typeId: data.typeId, loading: false,loadingSave: false, basePay: data.basePay });
  }

  async populateEmployeeTypes() {
    const token = await authService.getAccessToken();
    const response = await fetch('api/employeetype', {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    console.log(data);
    this.setState({ employeetypes: data, loading: false });
  }

  async getEmployeeTypeLabel(id) {
    this.setState({ loading: true,loadingSave: false });
    const token = await authService.getAccessToken();
    const response = await fetch('api/employeetype/' + id, {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ label: data.payLabel, loading: false });
  }
}
